<template>
  <div class="about">
    <div>
      <sui-input placeholder="Search..." icon="search" v-model="data.search" v-on:keyup.enter="search"  />
      <sui-button primary @click.prevent="search">Поиск</sui-button>
      <sui-divider />

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
      <div v-else-if="profile.games">

      <GameListTable :gameslist="profile.games" :platformlist="platformlist" :genrelist="genrelist"/>        
      
      </div>

    </div>
  </div>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from "vue";
import { State, Action, Getter } from "vuex-class";
import Component from "vue-class-component";
import { Route } from 'vue-router'
import { GameState, Game, GameRequest } from "../store/game/types";
import { Prop, Watch } from "vue-property-decorator";
import GameListTable from "../components/GameList.vue"

interface Data{
        loading: boolean ;
        page: string | (string | null)[];
        search: string | undefined | null;
    }

@Component({
  components: {
    GameListTable
  }
})
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
 
  
  async created() {
    // получение данных после монтирования компонента
    await this.fetchData();
  }
  

  @Watch("$route.query", { immediate: true })
   onUrlChange(newVal: Route) {
    
      this.fetchData()
     
    }

 
  private async fetchData()
  {
    this.data.loading = true;
    const newName = this.$route.query.name;
    const newPage = this.$route.query.page;

    if (newName){
      this.data.search = String(newName);
    }
    else this.data.search = null;

    if (newName || newPage) {
      this.request.name = this.data.search;
      this.request.page = newPage ? String(newPage) : null;
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
