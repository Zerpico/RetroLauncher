import { ActionTree } from "vuex";
import axios from "axios";
import { GameState, Game, GameRequest } from "./types";
import { RootState } from "../types";

export const actions: ActionTree<GameState, RootState> = {
  async fetchGames({ commit }): Promise<any> {
    await axios({
      url: "https://retro.khudaev.ru/api/Games/GetList",
    }).then(
      (response) => {
        console.log("Get fetch games");
        const games: Game[] = response && response.data.data.games;
        const currentPage = response && response.data.pages.current;
        const maxPage = response && response.data.pages.max;
        commit("gameLoaded", games);
        commit("gameLoadedCurrentPage", currentPage);
        commit("gameLoadedMaxPage", maxPage);
      },
      (error) => {
        console.log(error);
        commit("gameError");
      }
    );
  },
  async fetchGamesByName({ commit }, request: GameRequest): Promise<any> {
    await axios({
      url: "https://retro.khudaev.ru/api/Games/GetByName?Name="+request.name,
    }).then(
      (response) => {
        console.log("Get fetch games");
        const games: Game[] = response && response.data.data.games;
        const currentPage = response && response.data.pages.current;
        const maxPage = response && response.data.pages.max;
        commit("gameLoaded", games);
        commit("gameLoadedCurrentPage", currentPage);
        commit("gameLoadedMaxPage", maxPage);
      },
      (error) => {
        console.log(error);
        commit("gameError");
      }
    );
  },
};
