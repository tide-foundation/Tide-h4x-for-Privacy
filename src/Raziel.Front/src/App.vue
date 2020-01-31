<template>
  <div id="app" :style="{ height: windowHeight }">
    <div id="brute-force">
      <section v-if="attempts != null">
        {'totalAttempts':
        <span class="success">{{ attempts.total }}</span
        >,'attemptsThisHour':
        <span class="success">{{ attempts.thisHour }}</span
        >}
      </section>
    </div>
    <section id="top-content">
      <section id="art">
        <l n=" 1" c="/****************************/"></l>
        <l n=" 2" c="//     Tide Privacy h4x     //"></l>
        <l n=" 3" c="//        .--------.        //"></l>
        <l n=" 4" c="//       / .------. \       //"></l>
        <l n=" 5" c="//      / /        \ \      //"></l>
        <l n=" 6" c="//      | |        | |      //"></l>
        <l n=" 7" c="//     _| |________| |_     //"></l>
        <l n=" 8" c="//   .' |_|        |_| '.   //"></l>
        <l n=" 9" c="//   '._____ ____ _____.'   //"></l>
        <l n="10" c="//   |     .'____'.     |   //"></l>
        <l n="11" c="//   '.__.'.'    '.'.__.'   //"></l>
        <l
          n="12"
          c="//   '.__  | <span style='color:#7fcbe8'>TIDE</span> |  __.'   //"
        ></l>
        <l n="13" c="//   |   '.'.____.'.'   |   //"></l>
        <l n="14" c="//   '.____'.____.'____.'   //"></l>
        <l n="15" c="//   '.________________.'   //"></l>
        <l n="16" c="/****************************/"></l>
        <l n="17"></l>
      </section>

      <l n="18" c="// Enter login details"></l>
      <form @submit.prevent="getKeys">
        <l n="19" :edited="username != ''">
          <div class="v">var&nbsp;</div>
          <p class="variable">username</p>
          <p>&nbsp;=&nbsp;</p>
          <p class="s">'</p>
          <input v-model="username" type="text" />
          <p class="s">'</p>
          <p>;</p>
        </l>
        <l n="20" :edited="password != ''">
          <div class="v">var&nbsp;</div>
          <p class="variable">password</p>
          <p>&nbsp;=&nbsp;</p>
          <p class="s">'</p>
          <input v-model="password" type="password" />
          <p class="s">'</p>
          <p>;</p>
        </l>
        <l n="21"></l>
        <l n="22">
          <button
            :class="{ disabled: loading, accepted: unlocked }"
            type="submit"
            v-html="btnText"
          ></button>
        </l>
      </form>
      <l n="23"></l>
      <l n="24"></l>
      <l n="25"></l>
      <l n="26" c="// Proof of ownership"></l>
      <l
        style="white-space: nowrap;"
        n="27"
        c="// Type a message below and it'll be signed automatically with the bitcoin key by 7 trustless nodes"
      ></l>
      <l n="28">
        <div class="v">const&nbsp;</div>
        <p class="variable">bitcoinId</p>
        <p>&nbsp;=&nbsp;</p>
        <p class="s">'</p>
        <span>
          <a
            class="success"
            target="_blank"
            :href="'https://www.blockchain.com/btc/address/' + bitcoinId"
            >{{ bitcoinId }}</a
          >
        </span>
        <p class="s">'</p>
        <p>;</p>
      </l>
      <l n="29">
        <div class="v">var&nbsp;</div>
        <p class="variable">message</p>
        <p>&nbsp;=&nbsp;</p>
        <p class="s">'</p>
        <input
          class="sign-input"
          v-model="signMsg"
          type="text"
          @focus="signMsgFocus"
          @blur="signMsgBlur"
        />
        <p class="s">'</p>
        <p>;</p>
      </l>
      <l n="30"></l>
      <l n="31">
        <button
          type="button"
          @click="sign"
          v-html="signBtnText"
          :class="{ disabled: signLoading || this.signMsg == '' }"
        ></button>
      </l>
      <l n="32"></l>
      <l n="33">
        <span v-html="progressBar"></span>
      </l>
      <l n="34">
        <div class="v">var&nbsp;</div>
        <p class="variable">result</p>
        <p>&nbsp;=&nbsp;</p>

        <p class="s">'</p>
        <span>
          <a class="success" target="_blank" :href="signUrl">
            {{ signResult != null ? signResult : "" }}
          </a>
        </span>
        <p class="s">'</p>

        <p>;</p>
      </l>
    </section>
    <section id="bottom-content" :class="{ expanded: expanded }">
      <consoleBox />
    </section>
    <transition mode="out-in" name="fade">
      <section class="modal" id="modal" v-if="unlocked">
        <section v-if="!decrypted">
          <p>
            First name:
            <span class="success">{{ user.firstName }}</span>
          </p>
          <p>
            Last name:
            <span class="success">
              <span class="success">{{ user.lastName }}</span>
            </span>
          </p>
          <p>
            Bitcoin key:
            <span class="success">
              <span class="success">{{ user.bitcoinPrivateKey }}</span>
            </span>
          </p>
          <p>
            Note:
            <span class="success">
              <span class="success">{{ user.note }}</span>
            </span>
          </p>

          <button @click="decryptUser">Decrypt</button>
        </section>

        <section v-if="decrypted">
          <div class="modal-line">
            <p>First name:</p>
            <input
              type="text"
              class="success modal-input"
              v-model="user.firstName"
            />
          </div>
          <div class="modal-line">
            <p>Last name:</p>
            <input
              type="text"
              class="success modal-input"
              v-model="user.lastName"
            />
          </div>
          <div class="modal-line">
            <p>Bitcoin key:</p>
            <input
              type="text"
              class="success modal-input"
              v-model="user.bitcoinPrivateKey"
            />
          </div>
          <div class="modal-line">
            <p>Note:</p>
            <input
              type="text"
              class="success modal-input"
              v-model="user.note"
            />
          </div>

          <button @click="save">Save</button>
        </section>
      </section>
    </transition>
  </div>
</template>

<script>
import l from "./components/Line.vue";
import consoleBox from "./components/Console.vue";
import sign from "./components/Sign.vue";

import config from "./assets/js/config.js";

// import "./assets/js/cryptide.js";
// import "./assets/js/tide.min.js";
export default {
  name: "app",
  components: { l, consoleBox, sign },
  data() {
    return {
      tide: Tide.Create,
      btnText: "Login",
      signBtnText: "Threshold Sign",
      signMsg: "Welcome to the world's fastest TSS!",
      signResult: null,
      signProgressTarget: 0,
      signLoading: false,
      signUrl: null,
      bitcoinId: "14jazAFJEBcacDiJs9xANqD8EHJTo8Exe4",
      currentProgress: 0,
      unlocked: false,
      currentId: 1000,
      loading: false,
      decrypted: false,
      username: "",
      password: "",
      expanded: false,
      windowHeight: "40vh",
      attempts: null,

      keys: {
        priv: "",
        pub: ""
      },
      auth: "",
      user: {
        firstName: "",
        lastName: "",
        bitcoinPrivateKey: "",
        note: ""
      }
    };
  },
  created() {
    this.tide.init(config.orkNodes, 32);
    this.getAttempts();
    this.$bus.$on("toggle", toggled => (this.expanded = toggled));
    window.addEventListener("resize", () => this.setWindowHeight());
    this.setWindowHeight();
  },
  computed: {
    authModel: function () {
      var model = {
        user: this.user,
        referrer: document.referrer
      };
      model.user.username = this.tide.hashUsername(this.username).username;
      return model;
    },
    progressBar: function () {
      if (this.currentProgress == 0) return '';

      var val = parseInt(this.currentProgress * 50);
      var bar = "<span class='progress-bracket'>[</span>";
      for (let i = 0; i < 50; i++) {
        if (i < val) bar += "<span class='progress-block'>#</span>";
        else bar += ".";
      }
      return bar + "<span class='progress-bracket'>]</span>";
    }
  },
  methods: {
    signMsgFocus() {
      if (this.signMsg == "Welcome to the world's fastest TSS!") this.signMsg = "";
    },
    signMsgBlur() {
      if (this.signMsg == "") this.signMsg = "Welcome to the world's fastest TSS!";
    },
    async getKeys() {
      if (this.loading) return;
      if (!this.expanded) this.$bus.$emit("trigger-expand");
      this.auth = "";
      this.$bus.$emit("logging in", true);
      this.loading = true;
      this.btnText = "Logging in...";
      const tokenId = this.nextId();
      const detailsId = this.nextId();

      try {
        this.log(this.nextId(), "Gathering user token", "log");
        const tokenResponse = await this.tideRequest(
          `${config.vendorEndpoint}/Token/`,
          this.authModel
        );
        this.auth = tokenResponse.token;
        this.log(this.nextId(), "Fetching fragments", "log");
        
        const flow = new cryptide.TAuthFlow(config.signNodes.slice(0, 4), this.username);
        const tideResult = await flow.logIn(this.password);

        this.keys = {priv: tideResult.priv.toString(), pub: tideResult.pub.toString()};

        this.auth = await this.tide.processEncryption(
          false,
          this.auth,
          this.keys.priv
        );
        this.log(this.nextId(), "Gathering user details", "log");
        this.user = await this.tideRequest(
          `${config.vendorEndpoint}/getdetails/`,
          this.authModel
        );

        this.log(this.nextId(), "You have successfully logged in", "success");
        this.unlocked = true;
        this.btnText = "Accepted";
      } catch (error) {
        this.btnText = "Login";
        this.log(this.nextId(), "Login failed", "error");
      } finally {
        this.loading = false;
      }
    },
    async save() {
      try {
        var data = this.user;
        await this.encryptUser();

        var saveResponse = await this.tideRequest(
          `${config.vendorEndpoint}/Save/`,
          this.authModel
        );
      } catch (errorThrown) {
        console.log("Failed updating.");
      }
    },
    tideRequest(url, data) {
      const self = this;
      return new window.Promise(async function (resolve, reject) {
        try {
          const http = new XMLHttpRequest();
          http.onreadystatechange = function () {
            if (this.readyState === 4) {
              if (this.status === 200) {
                return resolve(JSON.parse(this.responseText));
              } else return reject(this.error);
            }
          };

          http.open(data != null ? "POST" : "GET", url);
          http.setRequestHeader("Authorization", "Bearer " + self.auth);
          if (data != null) {
            http.setRequestHeader(
              "Content-type",
              "application/json; charset=utf-8"
            );
            http.send(JSON.stringify(data));
          } else {
            http.send();
          }
        } catch (error) {
          return reject(error);
        }
      });
    },
    async decryptUser() {
      this.user.firstName = await this.tide.processEncryption(
        false,
        this.user.firstName,
        this.keys.priv
      );

      this.user.lastName = await this.tide.processEncryption(
        false,
        this.user.lastName,
        this.keys.priv
      );
      this.user.bitcoinPrivateKey = await this.tide.processEncryption(
        false,
        this.user.bitcoinPrivateKey,
        this.keys.priv
      );
      this.user.note = await this.tide.processEncryption(
        false,
        this.user.note,
        this.keys.priv
      );
      this.decrypted = true;
    },
    async encryptUser() {
      this.user.firstName = await this.tide.processEncryption(
        true,
        this.user.firstName,
        this.keys.pub
      );
      this.user.lastName = await this.tide.processEncryption(
        true,
        this.user.lastName,
        this.keys.pub
      );
      this.user.bitcoinPrivateKey = await this.tide.processEncryption(
        true,
        this.user.bitcoinPrivateKey,
        this.keys.pub
      );
      this.user.note = await this.tide.processEncryption(
        true,
        this.user.note,
        this.keys.pub
      );
      this.decrypted = false;
    },
    log(id, msg, type = "log") {
      document.dispatchEvent(
        new CustomEvent("log", { detail: { id: id, msg: msg, type: type } })
      );
    },
    nextId() {
      return this.currentId++;
    },
    setWindowHeight() {
      let vh = window.innerHeight * 0.01;
      this.windowHeight = `${vh * 100}px`;
    },
    async getAttempts() {
      try {
        this.attempts = await this.tideRequest(config.portalEndpoint);
        setTimeout(this.getAttempts, 50000);
      } catch (error) {
        // Ignored
      }
    },
    async sign() {
      this.signLoading = true;
      this.signBtnText = "Signing...";
      this.signProgressTarget = 0;
      this.signResult = null;

      var interval = setInterval(() => {
        if (this.currentProgress < this.signProgressTarget) this.currentProgress += 0.03
      }, 200)

        var result = await new cryptide.TSignFlow(config.signNodes.slice(0, 4), 'admin').sign(this.signMsg);
        this.signUrl = `https://brainwalletx.github.io/#verify?vrAddr=${
        result.address
        }&vrMsg=${encodeURI(this.signMsg)}&vrSig=${encodeURI(result.signature)}`;

      this.signResult = result.signature;
      this.signLoading = false;
      this.signBtnText = "Threshold Sign"
      clearInterval(interval);
      this.currentProgress = 0;
    }
  }
};
</script>

<style lang="scss">
@import url("https://fonts.googleapis.com/css?family=Roboto+Mono");

body {
  background-color: #232323;
  color: #a5a5a5;
  font-size: 15px;
  font-family: "Roboto Mono", monospace;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}

*::-webkit-scrollbar-track {
  -webkit-box-shadow: inset 0 0 4px rgba(0, 0, 0, 0.3);
  background-color: #f5f5f5;
}

*::-webkit-scrollbar {
  width: 4px;
  height: 4px;
  background-color: #f5f5f5;
}

*::-webkit-scrollbar-thumb {
  background-color: #9073be;
}

a {
  color: #a47326;
}

#art {
  white-space: pre !important;
}

.c {
  color: #517535;
}

.v {
  color: #187abb;
}

.success {
  color: #bd93f9;
}

.variable {
  color: #7fcbe8;
}

.disabled {
  pointer-events: none;
  background-color: gray;
}

.accepted {
  background: #519548;
}

.s {
  color: #a47326;
  margin: 0px !important;
  padding: 0px !important;
}

.quotation {
  margin: 0px !important;
  padding: 0px !important;
}

input {
  margin: 0px;
  border: 0px;
  background: transparent;
  width: 110px;

  color: #a47326;
  font-family: "Roboto Mono", monospace;
}

input:focus {
  outline: none;
}

::selection {
  background: rgba($color: #264f78, $alpha: 0.5);
}

button {
  background-color: #0e639c;
  border: 0px;
  color: #ffffff;
  padding: 0 25px 2px 25px;
  height: 20px;
  font-family: "Roboto Mono", monospace;
  cursor: pointer;
  min-width: 130px;
  font-size: 12px;
}

button:focus {
  outline: none;
}

button:hover {
  background-color: #1177bb;
}

body {
  padding: 0px;
  margin: 0px;
  overflow: hidden;
}
#app {
  display: flex;
  flex-direction: column;

  margin-top: 0px !important;
}
#top-content {
  flex: 1;
  overflow: auto;
  padding: 10px 0;
}
#bottom-content {
  height: 35px;
  transition: height 0.25s ease-in;
}

.expanded {
  height: 170px !important;
}

.modal {
  position: absolute;
  width: 100%;
  height: 100vh;
  background-color: rgba(23, 23, 23, 0.7);
  display: flex;
  justify-content: center;
  align-items: center;
  section {
    word-break: break-word;
    padding: 10px;
    color: white;

    width: 90%;

    background-color: #282a36;
    border: 1px solid #bd93f9;
    transition: height 0.25s ease-in;
    p {
      word-break: break-word;
    }
  }
}

.modal-input {
  flex: 2;
}

.modal-line {
  display: flex;
  align-items: center;
  width: 100%;
}

#brute-force {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: 20px;
  font-size: 10px;
  background: #282a36;
  border-bottom: 1px #b08fe4 solid;
}

#bitcoin-id-input {
  width: 275px;
}

.progress-bracket {
  color: white;
}

.progress-block {
  color: #b08fe4;
}

.sign-input {
  width: 280px;
}
</style>
