import HomeView from "@/views/home/HomeView.vue";
import LoginView from "@/views/login/LoginView.vue";
// --- plop:import

export const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView
  },
  {
    path: "/login",
    name: "login",
    component: LoginView
  }
];
