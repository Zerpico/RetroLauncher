<template>
  <div class="about">
    <div>

      <sui-input placeholder="Search..." icon="search" v-model="data.search"  />
      <sui-button primary @click.prevent="search">Поиск</sui-button>

      <sui-message v-if="profile.error" info>
        Error while fetch games list
      </sui-message>

      <h4><sui-button>p</sui-button> {{ profile.currentPage }} of {{ profile.maxPage }} <sui-button>n</sui-button> </h4>
	  	 
      <sui-loader v-if="data.loading" active />

      <div v-if="profile.games">
      <sui-list divided relaxed>
        <sui-list-item v-for="game in gameslist" :key="game.id">
          <sui-list-content>
            <a href="#" is="sui-list-header">
              <sui-grid>
                <sui-grid-row>
                  <sui-grid-column :width="2">
                    <img :src="game.links[0].url" style="width: 8em" />
                  </sui-grid-column>
                  <sui-grid-column :width="13">
                    <sui-list>
                      <sui-list-item style="color: #3f3f3f">{{game.name}}</sui-list-item>
                      <sui-list-item v-if="game.alternative !== game.name" style="color: #7a7a7a">{{ game.alternative }}</sui-list-item>                      
                      <sui-list-item style="color: rgb(141 74 74);">
                       <p>
                         <div class="listgenre">
                          <span v-for="genre of game.genres" :key="genre.id">
                            {{genrelist[genre].name}}
                          </span>
                         </div>
                       
                      </sui-list-item>
					  
					  <sui-list-item style="color: rgb(41 174 74);">
						{{ platformlist[game.platform].name }}
					  </sui-list-item>

                      <sui-list-item style="color: rgb(156 147 114)">{{ game.year }}</sui-list-item>
                      <sui-list-item style="color: #3f3f3f">{{ game.publisher }}</sui-list-item>
                      <sui-list-item style="color: #3f3f3f"><sui-rating :rating="game.rating" :max-rating="5" /></sui-list-item>
                    </sui-list>
                  </sui-grid-column>
                </sui-grid-row>
              </sui-grid>
            </a>
          </sui-list-content>
        </sui-list-item>
      </sui-list>
      </div>

    </div>
  </div>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from "vue";
import { State, Action, Getter } from "vuex-class";
import Component from "vue-class-component";
import { GameState, Game, GameRequest } from "../store/game/types";
import { Prop } from "vue-property-decorator";
const namespace = "game";

interface Data{
        loading: boolean ;
        gameid: string | (string | null)[]
        search: string
    }

@Component
export default class GameList extends Vue {
  private data: Data = {
          loading: true,
          gameid: "",
          search: ""
      }; 

  private request: GameRequest = 
  {
    name: "",
    genre: null,
    platform: null
  }

  @State("game") 
  profile!: GameState;
  @Action("fetchGames")
  private fetchGames: any;
  @Action("fetchGamesByName")
  private fetchGamesByName: any;
  
  @Getter("gamelist")
  private gameslist!: Game[];
  @Getter("platformlist")
  private platformlist!: [];
  @Getter("genrelist")
  private genrelist!: [];
 
  
  async mounted() {
    // получение данных после монтирования компонента
    this.data.gameid = this.$route.query.id;
    await this.fetchGames();
    this.data.loading = false;
  }

  private search()
  {
    this.request.name = this.data.search;
    this.data.loading = true;
    this.fetchGamesByName(this.request)
    this.data.loading = false;
  }

  // вычисляемое свойство email пользователя
}
</script>

<style>
.ui.divided.list > .item:hover {
  border-top: 1px solid rgba(34, 36, 38, 0.15);
  background: var(--custom-product-tile-bg, var(--theme-bg-secondary, #c0c0c0));
}
.list span:not(:last-child)::after {
  content: ',';
}
</style>
