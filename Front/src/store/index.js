import Vue from "vue";
import Vuex from "vuex";
import gamelist from "./modules/gamelist";
import game from "./modules/game";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {},
  mutations: {},
  actions: {},
  modules: {
    gamelist,
    game
  },
});
