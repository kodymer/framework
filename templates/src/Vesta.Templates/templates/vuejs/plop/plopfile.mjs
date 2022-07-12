const importFlag = "// --- plop:import";
const routesFlag = "// --- plop:routes";

export default function (plop) {
  plop.setWelcomeMessage("ASISA VESTA: Please choose an option:");

  // TODO: chequear con env
  plop.setHelper("appPrefix", () => "asisa");

  // Views (main component + router)
  plop.setGenerator(spacePad("view"), {
    description: "generates a basic view",
    prompts: [
      {
        type: "input",
        name: "name",
        message:
          '=> specify view name (snake-case, without "view" or "vue" suffixes): ',
      },
      {
        type: "input",
        name: "route",
        message:
          '=> specify route (snake-case, without "/". Leave blank to use view name as route): ',
      },
    ],
    actions: [
      {
        type: "add",
        path: "../src/views/{{ name }}/{{ ViewName name }}.vue",
        templateFile: "templates/view/ExampleView.vue.hbs",
      },
      {
        type: "add",
        path: "../src/views/{{ name }}/{{ ViewName name }}.spec.ts",
        templateFile: "templates/view/ExampleView.spec.ts.hbs",
      },
      {
        type: "modify",
        path: "../src/router/routes.ts",
        pattern: importFlag,
        template: `import {{ ViewName name }} from "{{ ViewRoute name }}";\n${importFlag}`,
      },
      {
        type: "modify",
        path: "../src/router/routes.ts",
        pattern: routesFlag,
        template: `{\n    path: "/{{ Default route name }}",\n    name: "{{ name }}",\n    component: {{ ViewName name }},\n  },\n  ${routesFlag}`,
      },
      {
        type: "[SUCCESS]",
        message:
          "{{ ViewName name }} created. Remember to remove unused comments (except plop:*)",
      },
    ],
  });

  // Component for a view
  plop.setGenerator(spacePad("view component"), {
    description: "generates a component for a existing view",
    prompts: [
      {
        type: "input",
        name: "viewName",
        message:
          '=> specify view name (snake-case, without "view" or "vue" suffixes). It must exists: ',
      },
      {
        type: "input",
        name: "name",
        message:
          '=> specify component name (snake-case, without "component" or "vue" suffixes): ',
      },
    ],
    actions: [
      {
        type: "add",
        path: "../src/views/{{ viewName }}/components/{{ name }}/{{ ComponentName name }}.vue",
        templateFile: "templates/view-component/ExampleComponent.vue.hbs",
      },
      {
        type: "add",
        path: "../src/views/{{ viewName }}/components/{{ name }}/{{ ComponentName name }}.spec.ts",
        templateFile: "templates/view-component/ExampleComponent.spec.ts.hbs",
      },
      {
        type: "modify",
        path: "../src/views/{{ viewName }}/{{ ViewName viewName }}.vue",
        pattern: importFlag,
        template: `import {{ ComponentName name }} from "{{ RelativeComponentRoute name }}";\n${importFlag}`,
      },
      {
        type: "[SUCCESS]",
        message:
          "{{ ComponentName name }} created. Remember to remove unused comments (except plop:*)",
      },
    ],
  });

  // Core component
  plop.setGenerator(spacePad("core component"), {
    description: "generates a core component (eg. header, main navbar, etc)",
    prompts: [
      {
        type: "input",
        name: "scope",
        message: '=> specify component scope (snake-case, eg. "ui", "forms"): ',
      },
      {
        type: "input",
        name: "name",
        message:
          '=> specify component name (snake-case, without "component" or "vue" suffixes): ',
      },
    ],
    actions: [
      {
        type: "add",
        path: "../src/core/components/{{ scope }}/{{ name }}/{{ ComponentName name }}.vue",
        templateFile: "templates/core-component/ExampleComponent.vue.hbs",
      },
      {
        type: "add",
        path: "../src/core/components/{{ scope }}/{{ name }}/{{ ComponentName name }}.spec.ts",
        templateFile: "templates/core-component/ExampleComponent.spec.ts.hbs",
      },
      {
        type: "[SUCCESS]",
        message:
          "{{ ComponentName name }} (core) created. Remember to remove unused comments (except plop:*)",
      },
    ],
  });

  // Exit script
  plop.setGenerator(spacePad("exit"), {
    description: "exits the script",
    prompts: [],
    actions: [
      {
        type: "[INFO]",
        message: "No action done",
      },
    ],
  });

  // Helpers and shared actions
  plop.setHelper("Default", (value1, value2) => {
    return value1 || value2;
  });
  plop.setActionType("[SUCCESS]", (answers, config, plop) => {
    return plop.renderString(config.message, answers);
  });
  plop.setActionType("[INFO]", (answers, config, plop) => {
    return plop.renderString(config.message, answers);
  });
  plop.setHelper("ViewName", (name) => {
    return `${pascalName(name)}View`;
  });
  plop.setHelper("ViewRoute", (name) => {
    return `@/views/${name}/${pascalName(name)}View.vue`;
  });
  plop.setHelper("ComponentName", (name) => {
    return `${pascalName(name)}Component`;
  });
  plop.setHelper("RelativeComponentRoute", (name) => {
    return `./components/${name}/${pascalName(name)}Component.vue`;
  });
  plop.setHelper("ViewComponentRoute", (viewName, name) => {
    return `@/views/${viewName}/components/${name}/${pascalName(
      name
    )}Component.vue`;
  });
  plop.setHelper("CoreComponentRoute", (scope, name) => {
    return `@/core/components/${scope}/${name}/${pascalName(
      name
    )}Component.vue`;
  });
}
// Tools
const pascalName = (text) =>
  text
    .match(/[a-z]+/gi)
    .map((word) => {
      return word.charAt(0).toUpperCase() + word.substr(1).toLowerCase();
    })
    .join("");
const spacePad = (text, places = 30) => text.padStart(places, " ");
