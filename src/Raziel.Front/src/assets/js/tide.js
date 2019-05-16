/* 
 * Tide Protocol - Infrastructure for the Personal Data economy
 * Copyright (C) 2019 Tide Foundation Ltd
 *	
 * This program is free software and is subject to the terms of
 * the Tide Community Open Source Licence as published by the 
 * Tide Foundation Limited. You may modify it and redistribute 
 * it in accordance with and subject to the terms of that licence.
 * This program is distributed WITHOUT WARRANTY of any kind, 
 * including without any implied warranty of MERCHANTABILITY or 
 * FITNESS FOR A PARTICULAR PURPOSE.  
 * See the Tide Community Open Source Licence for more details.
 * You should have received a copy of the Tide Community Open 
 * Source Licence along with this program.  
 * If not, see https://tide.org/licenses/tcosl-1.0.en.html
 */

import './cryptide.js'

export default class Tide {
  constructor(orkNodes) {
    this.nodeArray = orkNodes;
    this.hashes = [];
  }

  getTideCredentials(username, password) {
    var self = this;
    return new window.Promise(
      async function (resolve, reject) {
        const id = nextId();
        log(id, `gathering user nodes...`)
        try {
          const saltAndUser = self.hashUsername(username);

          // Gather the nodes the user used to register with Tide
          const userNodes = await tideRequest(`${self.nodeArray[0]}/nodes`, {
            username: saltAndUser.username
          });

          log(id, `Gathered user nodes. Count: ${userNodes.length}`)
          log(id, `Creating password fragments`)
          self.hashes = await elGamal.hashPasswords(password, saltAndUser.salt, userNodes.map(n => n.ork_url));

          // Get the fragments from each node
          const fragmentResult = await gatherIdentFragments(userNodes, saltAndUser.username, password, self.hashes);

          return resolve(fragmentResult);
        } catch (thrownError) {
          log(nextId(), thrownError, 'error')
          return reject();
        }
      }
    );
  }

  processEncryption(encrypt, data, key) {
    return new window.Promise(
      async function (resolve, reject) {
        try {
          if (data == '' || data == null) return resolve('');
          return resolve(encrypt ? elGamal.encrypt(data, key) : elGamal.decrypt(data, key))
        } catch (error) {
          return reject("Incorrect private key");
        }
      });
  }

  hashUsername(data) {
    var salt = elGamal.hashSha(data)
    var username = elGamal.hashSha(salt)
    return {
      salt: salt,
      username: username
    };
  }
}


function gatherIdentFragments(nodes, username, password, hashes) {
  return new window.Promise(
    async function (resolve, reject) {

      // Seed transient keys
      log(nextId(), `Generating transient key-pair for transmission encryption`)
      const [priv, pub] = elGamal.getKeys(32);

      const model = {
        username: username,
        publicKey: pub,
        passwordHash: ""
      };

      var frags = [];
      const id = nextId();
      const results = nodes.map((n) => tideRequest(`${n.ork_url}/login`, appendHash(model, n.ork_url, hashes)));
      log(id, `Gathering fragments. ${getProgress(frags.length, nodes.length)}`)

      for (const r of results) {

        await r.then((content) => {

          frags.push(content);
          log(id, `Gathering fragments. ${getProgress(frags.length, nodes.length)}`)
          if (frags.length == 17) {

            log(id, `Finished gathering fragments`, 'success')

            return resolve({
              priv: elGamal.combineKeys(frags.map(f => elGamal.decrypt(f.vendorFragment.private_key_frag, priv))),
              pub: frags[0].vendorFragment.public_key
            });
          }
        }).catch((e) => {
          return reject(e);
        });
      }
    });
}

function tideRequest(url, data) {
  return new window.Promise(
    async function (resolve, reject) {
      const http = new XMLHttpRequest();
      http.onreadystatechange = function () {
        if (this.readyState === 4) {
          if (this.status === 200) {

            const content = JSON.parse(this.responseText);
            if (content.success) return resolve(content.content);
            return reject(content.error);
          } else return reject(this.error);
        }
      };
      http.open(data != null ? "POST" : "GET", url);
      if (data != null) {
        http.setRequestHeader("Content-type", "application/json; charset=utf-8");
        http.send(JSON.stringify(data));
      } else {
        http.send();
      }
    });
}

function appendHash(model, node, hashes) {
  model.passwordHash = hashes.find(h => h.server == node).pass;
  return model;
}

var currentId = 0;

function nextId() {
  return currentId++;
}

function log(id, msg, type = 'log') {
  document.dispatchEvent(new CustomEvent("log", {
    detail: {
      id: id,
      msg: msg,
      type: type,
    }
  }));
}

function getProgress(cur, max) {
  var s = "<span style='color:#bd93f9'>|";
  for (var i = 0; i < cur; i++) s += "⣿";
  for (var i = 0; i < max - cur; i++) s += "⣀";
  s += "|</span>";
  return s;
}
