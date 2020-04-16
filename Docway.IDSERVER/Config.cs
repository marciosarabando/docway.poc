// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Docway.IDSERVER
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api1", "My API #1"),
                new ApiResource("mvc1", "My MVC #1")
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "api1",
                    ClientName = "Minha API 1",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowAccessTokensViaBrowser = true,

                    RequireConsent = false,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { "http://localhost:5001/signin-oidc" },

                    AllowedScopes =
                    { 
                        "api1",
                        "openid",
                        "profile",
                        "offline_access"
                    },
                    
                    //Refresh Token Settings
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 60 * 60 * 24 * 365
                },

                new Client
                {
                    ClientId = "mvc1",
                    ClientName = "Minha MVC App 1",

                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    AllowAccessTokensViaBrowser = true,
                    
                    RequireConsent = false,

                    ClientSecrets = { new Secret("segredo".Sha256()) },

                    RedirectUris = {
                        "http://localhost:5001/signin-oidc",
                    },
                    //LogoutUri = "http://localhost:9020/signout-oidc",
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:5001/signout-callback-oidc"
                    },

                    AllowedScopes = { "openid", "profile", "mvc1", "offline_access" },

                    //Refresh Token Settings
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 60 * 60 * 24 * 365
                }

            };
    }
}