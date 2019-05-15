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

using System;
using System.Collections.Generic;
using EosSharp;
using EosSharp.Api.v1;
using Microsoft.Extensions.Logging;
using Raziel.Library.Classes;
using Raziel.Library.Models;
using Action = EosSharp.Api.v1.Action;

namespace Raziel.Ork.Models {
    public class EosAdminTideAuthentication : IAdminTideAuthentication {
        private readonly Eos _eos;
        private readonly ILogger _logger;
        private readonly Settings _settings;

        public EosAdminTideAuthentication(Settings settings, ILoggerFactory logger) {
            _settings = settings;
            _logger = logger.CreateLogger($"Ork-{settings.Account}");

            _eos = new Eos(new EosConfigurator {
                HttpEndpoint = _settings.BlockchainEndpoint,
                ChainId = _settings.BlockchainChainId,
                ExpireSeconds = 60,
                SignProvider = new DefaultSignProvider(_settings.EosPrivateKey)
            });
        }

        public TideResponse CreateAccount(string publicKey, string username, bool seed = false) {
            try {
                var usernameHash = username.ConvertToUint64();

                var account = $"tide{Helpers.RandomString(8)}";
                var result = _eos.CreateTransaction(new Transaction {
                    Actions = new List<Action> {
                        new Action {
                            Account = _settings.Onboarding,
                            Authorization = new List<PermissionLevel> {
                                new PermissionLevel {Actor = _settings.Account, Permission = "active"}
                            },
                            Name = "adduser",
                            Data = new {
                                account,
                                username = usernameHash
                            }
                        }
                    }
                }).Result;

                _logger.LogMsg("Created user account");
                return new TideResponse(true, account, null);
            }
            catch (Exception e) {
                _logger.LogMsg("Failed creating user account", ex: e);
                return new TideResponse(false, null, "Failed creating user account");
            }
        }

        public TideResponse AddFragment(AuthenticationModel model) {
            try {
                var usernameHash = model.Username.ConvertToUint64();

                var result = _eos.CreateTransaction(new Transaction {
                    Actions = new List<Action> {
                        new Action {
                            Account = _settings.Onboarding,
                            Authorization = new List<PermissionLevel> {
                                new PermissionLevel {Actor = _settings.Account, Permission = "active"}
                            },
                            Name = "addfrag",
                            Data = new {
                                ork_node = _settings.Account,
                                ork_url = model.SiteUrl,
                                username = usernameHash,
                                private_key_frag = AesCrypto.Encrypt(model.CvkPrivateFrag, _settings.Password),
                                public_key = model.CvkPublic,
                                pass_hash = AesCrypto.Encrypt(model.PasswordHash, _settings.Password),
                                ork_public = _settings.PublicKey
                            }
                        }
                    }
                }).Result;
                _logger.LogMsg("Added fragment for user", model);
                return new TideResponse(true, null, null);
            }
            catch (Exception e) {
                _logger.LogMsg("Failed adding fragment", model, e);
                return new TideResponse(false, null, e.Message);
            }
        }
    }
}