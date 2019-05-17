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

document.getElementById('submit-btn').onclick = async function () {
    try {
        const username = document.getElementById("username").value;

        const result = await tide.postCredentials(username, document.getElementById("password").value);
        console.log(result)
        var processedUsername = tide.hashUsername(username)


        const request = {
            user: {
                username: processedUsername.username,
                firstName: await tide.processEncryption(true, "matt", result.pub),
                lastName: await tide.processEncryption(true, "spencer", result.pub),
                bitcoinPrivateKey: await tide.processEncryption(true, "lol key", result.pub),
                note: await tide.processEncryption(true, "some note", result.pub),
                vendorPublicKey: result.pub
            },
            token: "RccuTZP0inPEpoKZb49WiSDQOaFs6K*T17S2fbK@!6JakfVMU8qOgbLAeDKe5AaT4kp7%0^98TO5OI1"
        };

        console.log(await tide.tideRequest(`https://your-vendor-endpoint.net/PostUser`, request));

        console.log(`Account created successfully`);
    } catch (error) {
        console.log(error);
    }
};