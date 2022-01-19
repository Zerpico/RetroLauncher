<template>
  <div class="about">
    <div>

      <sui-input placeholder="Search..." icon="search" v-model="data.search" v-on:keyup.enter="search"  />
      <sui-button primary @click.prevent="search">Поиск</sui-button>

      <sui-message v-if="profile.error" info>
        Error while fetch games list
      </sui-message>

      <h4>
        <sui-button compact content="Prev" style="margin-right:1rem" @click.prevent="prevpage" :class="profile.currentPage === 1 ? 'disabled' : ''" /> 
        {{ profile.currentPage }} 
        <b style="margin:0 1rem">of</b> {{ profile.maxPage }} 
        <sui-button compact content="Next" style="margin-left:1rem" @click.prevent="nextpage" :class="profile.currentPage === profile.maxPage ? 'disabled' : ''"/> 
      </h4>
	  	 
      <sui-loader v-if="data.loading" active />
      <div v-else>
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
  </div>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from "vue";
import VueRouter from 'vue-router';
import { State, Action, Getter } from "vuex-class";
import Component from "vue-class-component";
import { GameState, Game, GameRequest } from "../store/game/types";
import { Prop, Watch } from "vue-property-decorator";
const namespace = "game";

interface Data{
        loading: boolean ;
        page: string | (string | null)[];
        search: string | undefined | null;
    }

@Component
export default class GameList extends Vue {
  private data: Data = {
          loading: true,
          page: "",
          search: ""
      }; 
  

  private request: GameRequest = 
  {
    name: null,
    page: null,
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
    this.fetchData();
  }

  @Watch('$route')
    onPropertyChanged(value: boolean, oldValue: boolean) {
      if (value)
        this.fetchData()   
  }

 
  private async fetchData()
  {
    this.data.loading = true;
    if (this.$route.query.name){
      this.data.search = String(this.$route.query.name);
    }
    else this.data.search = null;

    const page = this.$route.query.page;
    if (this.data.search || page) {
      this.request.name = this.data.search;
      this.request.page = page ? String(page) : null;
      await this.fetchGamesByName(this.request);
    }
    else await this.fetchGames();
    this.data.loading = false;
  }

  private search()
  {
    this.$router.push({ name: "games", query : { name: this.data.search, page: "1" } });
  }

  private nextpage() {
    this.$router.push({ name: "games", query : { name: this.data.search , page: String(this.profile.currentPage+1)} });
  }

  private prevpage() {
    this.$router.push({ name: "games", query : { name: this.data.search, page: String(this.profile.currentPage-1)} });
  }

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
