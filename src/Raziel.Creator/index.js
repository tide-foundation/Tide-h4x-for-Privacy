/* 
 * Tide Protocol - Infrastructure for the Personal Data economy
 * Copyright (C) 2019 Tide Foundation Ltd
 * 
 * This program is free software and is subject to the terms of 
 * the Tide Community Open Source License as published by the 
 * Tide Foundation Limited. You may modify it and redistribute 
 * it in accordance with and subject to the terms of that License.
 * This program is distributed WITHOUT WARRANTY of any kind, 
 * including without any implied warranty of MERCHANTABILITY or 
 * FITNESS FOR A PARTICULAR PURPOSE.
 * See the Tide Community Open Source License for more details.
 * You should have received a copy of the Tide Community Open 
 * Source License along with this program.
 * If not, see https://tide.org/licenses_tcosl-1-0-en
 */

import Tide from 'tide-js'
import config from './src/assets/js/config'

const tide = new Tide(config.nodes, 32);

document.getElementById("username").value = `User${Math.floor(Math.random() * 1000000) + 1}`;


document.getElementById('submit-btn').onclick = async function () {
    try {
        const username = document.getElementById("username").value;

        const result = await tide.postCredentials(username, document.getElementById("password").value);
        console.log(result)
        var processedUsername = tide.hashUsername(username)

        const request = {
            user: {
                username: processedUsername.username,
                firstName: await tide.processEncryption(true, document.getElementById("first").value, result.pub),
                lastName: await tide.processEncryption(true, document.getElementById("last").value, result.pub),
                bitcoinPrivateKey: await tide.processEncryption(true, document.getElementById("key").value, result.pub),
                note: await tide.processEncryption(true, document.getElementById("notes").value, result.pub),
                vendorPublicKey: result.pub
            },
            token: config.vendorPassword
        };
        var postResult = await tide.tideRequest(`${config.vendorEndpoint}/PostUser`, request)
        console.log(postResult);

        console.log(`Account created successfully`);
    } catch (error) {
        console.log(error);
    }
};