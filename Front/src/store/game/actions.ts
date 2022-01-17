import { ActionTree } from "vuex";
import axios from "axios";
import { GameState, Game } from "./types";
import { RootState } from "../types";

export const actions: ActionTree<GameState, RootState> = {
  fetchGames({ commit }): any {
    axios({
      url: "https://retro.khudaev.ru/api/Games/GetList",
    }).then(
      (response) => {
        console.log("Get fetch games");
        const games: Game[] = response && response.data.data.games;
        commit("gameLoaded", games);
      },
      (error) => {
        console.log(error);
        commit("gameError");
      }
    );
  },
};
