# vesta-frontend

## Description

App Asisa Vesta is the new application for retentions and cancellations. The purpose of this app is to save all the intentions of retentions and contract cancellations in the company no matter the affected department and to decide if retention is applicable relying on defined rules.

This repository contains the frontend of the application. It is being developed in VueJS.

## Pre-requisites

- [NodeJS](https://nodejs.org) (recommended v16 or higher and LTS version)

## Recommended IDE setup

[VSCode](https://code.visualstudio.com/)

VSCode extensions:

- [ESLint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint)
- [Prettier - Code formatter](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)
- [DotENV](https://marketplace.visualstudio.com/items?itemName=mikestead.dotenv)
- [Vue 3 Support - All In One](https://marketplace.visualstudio.com/items?itemName=Wscats.vue)
- [Vue Language Features (Volar)](https://marketplace.visualstudio.com/items?itemName=vue.volar)
- [TypeScript Vue Plugin (Volar)](https://marketplace.visualstudio.com/items?itemName=Vue.vscode-typescript-vue-plugin)
- [Stylelint](https://marketplace.visualstudio.com/items?itemName=stylelint.vscode-stylelint)

VSCode project settings (.vscode/settings.json):

```json
{
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.formatOnSave": true,
  "editor.rulers": [80],
  "[scss]": {
    "editor.defaultFormatter": "vscode.css-language-features"
  }
}
```

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Run Unit Tests with [Vitest](https://vitest.dev/)

```sh
npm run test:unit # or `npm run test:unit:coverage` for getting test coverage
```

### Run End-to-End Tests with [Cypress](https://www.cypress.io/)

```sh
npm run build
npm run test:e2e # or `npm run test:e2e:ci` for headless testing
```

### Lint + Prettify

- [ESLint](https://eslint.org/)
- [SonarJS](https://github.com/SonarSource/SonarJS)
- [Prettier](https://prettier.io/)

```sh
npm run lint
```

- [Stylelint](https://stylelint.io/)

```sh
npm run lint:css
```

Both commands have the suffix ":fix" for automatic fixing

```sh
npm run lint:fix
npm run lint:css:fix
```

### Husky

Husky (https://typicode.github.io/husky/#/) is integrated in this project for git-hooks execution and configured to lint and prettify staged files before commiting and eventually stop commit action.
