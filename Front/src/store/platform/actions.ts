import { ActionTree } from "vuex";
import axios from "axios";
import { PlatformState, Platform } from "./types";
import { RootState } from "../types";

export const actions: ActionTree<PlatformState, RootState> = {
  fetchPlatforms({ commit }): any {
    axios({
      url: "https://retro.khudaev.ru/api/Platforms/GetList",
    }).then(
      (response) => {
        console.log("Get getch platforms");
        const payload: Platform[] = response && response.data.data.platforms;
        commit("platformLoaded", payload);
      },
      (error) => {
        console.log(error);
        commit("platformError", error);
      }
    );
  },
};
