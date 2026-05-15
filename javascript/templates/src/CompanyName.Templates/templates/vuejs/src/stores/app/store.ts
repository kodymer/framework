import { defineStore } from "pinia";
import { initialAppState, type IntAppState } from "./state";

export const useAppStore = defineStore({
  id: "app",
  state: (): IntAppState => initialAppState,
  getters: {},
  actions: {},
});
