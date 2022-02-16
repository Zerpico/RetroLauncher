import { ActionTree } from "vuex";
import axios from "axios";
import { GenreState, Genre } from "./types";
import { RootState } from "../types";

export const actions: ActionTree<GenreState, RootState> = {
  fetchGenres({ commit }): any {
    axios({
      url: "https://retro.khudaev.ru/api/Genres/GetList",
    }).then(
      (response) => {
        console.log("Get getch genres");
        const payload: Genre[] = response && response.data.data.genres;
        commit("genreLoaded", payload);
      },
      (error) => {
        console.log(error);
        commit("genreError");
      }
    );
  },
};
