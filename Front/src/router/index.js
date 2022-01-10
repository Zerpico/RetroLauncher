import Vue from "vue";
import VueRouter from "vue-router";
import Games from "../views/Games.vue";
import Game from "../views/Game.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    redirect: '/games'
  },
  {
    path: "/games",
    name: "games",
    component: Games,
  },
  {
    path: "/game/:id",
    name: "game",
    component: Game,
  },
  {
    path: "/about",
    name: "about",
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue"),
  },
];

const router = new VueRouter({
  routes,
  mode: "history"
});

export default router;
