export default {
    state: {
      gamelists: []
    },
    mutations: {     
        updateGame(state, game) {
            state.gamelists = game;
          },
    },
    actions: {      
      async fetchGameById(ctx, id) {
        const res = await fetch(
          "https://retro.khudaev.ru/api/Games/GetById?Id=" + id
        );
        let games = null;       
        if (res.ok) {
          const gamelist = await res.json();
          games = gamelist.data.games;        
        }
        ctx.commit("updateGame", games);      
      },
    },
    modules: {},
    getters: {
      GameList(state) {
        return state.gamelists;
      },      
    },
  };
  