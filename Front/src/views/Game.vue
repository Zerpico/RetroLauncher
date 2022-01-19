<template>
    <div>
        <sui-grid divided="vertically">
            <sui-grid-row :columns="2">
            <sui-grid-column>
                <h3 style="margin-bottom: 0px;">{{ gameslist[0].name }}</h3>
                <h4 style="margin-top: 0px;color: #7a7a7a" v-if="gameslist[0].alternative !== gameslist[0].name">{{ gameslist[0].alternative }}</h4> 
            </sui-grid-column>
            <sui-grid-column>
                <sui-button color="green" @click.prevent="rungame">Запустить</sui-button>
            </sui-grid-column>
            </sui-grid-row>
        </sui-grid>
        

        <sui-rating :rating="gameslist[0].rating" :max-rating="5" />

        <div id="menu">
            <ul>
                <li v-for="link of gameslist[0].links" :key="link.id">
                    <img v-if="link.type !== 'rom'" :src="link.url" />
                </li>
            </ul>
        </div>

        <sui-table basic="very" celled collapsing>          
            <sui-table-body>
            <sui-table-row>
                <sui-table-cell><b>Платформа: </b></sui-table-cell>
                <sui-table-cell>{{ platformlist[gameslist[0].platform].name }}</sui-table-cell>
            </sui-table-row>
            <sui-table-row>
                <sui-table-cell><b>Жанр: </b></sui-table-cell>
                <sui-table-cell>
                    <span v-for="genre of gameslist[0].genres" :key="genre.id">
                            {{genrelist[genre].name}}
                          </span>
                </sui-table-cell>
            </sui-table-row>
            <sui-table-row>
                <sui-table-cell>
                    <b>Дата выхода: </b>
                </sui-table-cell>
                <sui-table-cell>
                    {{ gameslist[0].year }}
                </sui-table-cell>
            </sui-table-row>
            <sui-table-row>
                <sui-table-cell>
                    <b>Разработчик: </b>
                </sui-table-cell>
                <sui-table-cell>
                    {{ gameslist[0].publisher }}
                </sui-table-cell>
            </sui-table-row>
        </sui-table-body>
        </sui-table>
        <sui-container text-align="justified">
            {{ gameslist[0].annotation }}
        </sui-container>

        <div id="game">
        </div>
    </div>
</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import { State, Action, Getter } from "vuex-class";
import { GameState, Game, GameRequest } from "../store/game/types";

interface Data{
            loading: boolean ;       
    }

@Component
export default class GameView extends Vue {
  private data: Data = {
            loading: true           
    }; 

  @State("game") 
  profile!: GameState;
  @Action("fetchGameById")
  private fetchGameById: any;
  
  @Getter("gamelist")
  private gameslist!: Game[];
  @Getter("platformlist")
  private platformlist!: [];
  @Getter("genrelist")
  private genrelist!: [];

    //fetchGameById
  async created() {    
        //await this.fetchData();        
        await this.fetchGameById(this.$route.params.id);
  }

  rungame() {
      var gamediv = document.getElementById('game');
      if (gamediv.childNodes.length <= 0) {
        
        const url = this.gameslist[0].links[this.gameslist[0].links.length-1].url;


        var a = document.createElement('div');
        a.style = "width:640px;height:480px;max-width:100%;";
        var b = document.createElement('div');
		b.id = 'game';
		a.appendChild(b);
		document.body.appendChild(a);
		var script = document.createElement('script');
		script.innerHTML = "EJS_player = '#game'; EJS_gameName = '" + this.gameslist[0].name + "'; EJS_biosUrl = ''; EJS_gameUrl = '" + url + "'; EJS_core = '" + this.platformlist[this.gameslist[0].platform].alias + "'; EJS_pathtodata = 'data/'; ";
		document.body.appendChild(script);
		var script2 = document.createElement('script');
		script2.src = 'data/loader.js';
		gamediv.appendChild(script2);
      }
  }
}
</script>

<style>

.list span:not(:last-child)::after {
  content: ',';
}

#menu ul{
  list-style: none;
}
#menu li{
  display: inline;
  margin: 0 0.3rem
}

#game{
    margin-top: 3rem;
}
</style>