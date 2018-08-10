// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,

  auth: {
    clientID: 'IYbfge73nrVbuAa7227KffYCAeWk2Li0',
    domain: 'gold-mountain.eu.auth0.com', // e.g., you.auth0.com
    audience: 'http://localhost:5001', // e.g., http://localhost:3001
    redirectUri: 'http://localhost:4200/login-callback',
  },

  api: {
    clientApiUrl: 'http://localhost:5001/api',
    dataProviderUrl: 'http://localhost:5002/api',
  }
};
