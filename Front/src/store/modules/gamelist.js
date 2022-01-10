export default {
  state: {
    gamelists: [],
    currentpage: 0,
    maxpage: 0,
  },
  mutations: {
    updateGames(state, games) {
      state.gamelists = games;
    },
    updateCurrentPage(state, current) {
      state.currentpage = current;
    },
    updateMaxPage(state, max) {
      state.maxpage = max;
    },
  },
  actions: {
    async fetchGames(ctx) {
      const res = await fetch("https://retro.khudaev.ru/api/Games/GetList");
      let games = null;
      let current = null;
      let max = null;
      if (res.ok) {
        const gamelist = await res.json();
        games = gamelist.data.games;
        current = gamelist.pages.current;
        max = gamelist.pages.max;
      } else {
        alert("Ошибка HTTP: " + res.status);
      }
      ctx.commit("updateGames", games);
      ctx.commit("updateCurrentPage", current);
      ctx.commit("updateMaxPage", max);
    },
    async fetchGamesByPage(ctx, page) {
      const res = await fetch(
        "https://retro.khudaev.ru/api/Games/GetList?page=" + page
      );
      let games = null;
      let current = null;
      let max = null;
      if (res.ok) {
        const gamelist = await res.json();
        games = gamelist.data.games;
        current = gamelist.pages.current;
        max = gamelist.pages.max;
      }
      ctx.commit("updateGames", games);
      ctx.commit("updateCurrentPage", current);
      ctx.commit("updateMaxPage", max);
    },
  },
  modules: {},
  getters: {
    allGameList(state) {
      return state.gamelists;
    },
    currentPage(state) {
      return state.currentpage;
    },
    maxPage(state) {
      return state.maxpage;
    },
  },
};
