﻿// Tide Protocol - Infrastructure for the Personal Data economy
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
    public class TideResponse {
        public TideResponse(bool success, object content, string error) {
            Success = success;
            Content = content;
            Error = error;
        }

        public TideResponse(string error) {
            Success = false;
            Content = null;
            Error = error;
        }

        public TideResponse(object obj) {
            Success = true;
            Content = obj;
            Error = null;
        }

        public bool Success { get; set; }
        public object Content { get; set; }
        public string Error { get; set; }
    }
}