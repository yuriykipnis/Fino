export const environment = {
  production: true,

  auth: {
    clientID: 'GjPYL5xrrzPij20Lw16jIsqQ5mEGBagO',
    domain: 'gold-mountain.eu.auth0.com',
    audience: 'http://bewise1.westeurope.cloudapp.azure.com:5001',
    redirectUri: 'http://fino.co.il/login-callback'
    //redirectUri: 'https://fino.co.il/login-callback'
  },

  api: {
    clientApiUrl: 'http://bewise1.westeurope.cloudapp.azure.com:5001/api',
    dataProviderUrl: 'http://bewise1.westeurope.cloudapp.azure.com:5002/api',
  }
};
