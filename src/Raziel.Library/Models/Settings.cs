// Tide Protocol - Infrastructure for the Personal Data economy
// Copyright (C) 2019 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Source License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Source License for more details.
// You should have received a copy of the Tide Community Open 
// Source License along with this program.
// If not, see https://tide.org/licenses_tcosl-1-0-en

namespace Raziel.Library.Models {
    public class Settings {
        public string PublicKey { get; set; } //block chain???
        public string Account { get; set; } //block chain
        public string Onboarding { get; set; } //block chain
        public string EosPrivateKey { get; set; } //block chain
        public string BlockchainChainId { get; set; }
        public string BlockchainEndpoint { get; set; }
        public string Connection { get; set; } //sql database
        public string UsersTable { get; set; } //block chain table
        public string FragmentsTable { get; set; } //block chain table
        public string LogEndpoint { get; set; }
        public LoggerSettings LoggerSettings { get; set; }
        public string Key { get; set; } //another aes private ork key
        public string Password { get; set; } //aes private ork key
        public string EcDSAKey { get; set; } //ecdsa private key
        public string UserShare { get; set; } //ecdsa private key set for all user in sign and auth
        public string UserPublic { get; set; } //elgamal public key set for all user in auth
        public string UserPwd { get; set; } //ecdsa private key set for all user in sign and auth
    }
}