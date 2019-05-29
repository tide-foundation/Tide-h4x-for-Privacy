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
        public string PublicKey { get; set; }
        public string Account { get; set; }
        public string Onboarding { get; set; }
        public string EosPrivateKey { get; set; }
        public string Password { get; set; }
        public string BlockchainChainId { get; set; }
        public string BlockchainEndpoint { get; set; }
        public string Key { get; set; }
        public string Connection { get; set; }
        public string UsersTable { get; set; }
        public string FragmentsTable { get; set; }
        public string LogEndpoint { get; set; }
        public LoggerSettings LoggerSettings { get; set; }
    }

}