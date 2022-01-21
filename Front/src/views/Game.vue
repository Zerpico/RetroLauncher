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
                    <img v-if="link.type !== 'rom'" :src="link.url" style="max-height: 144px" />
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

        <div id="gamediv" style="margin-top:0.5rem">
        </div>
    </div>
</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import { State, Action, Getter } from "vuex-class";
import { GameState, Game, Link, GameRequest } from "../store/game/types";
import { Platform } from "../store/platform/types";

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
  private platformlist!: Platform[];
  @Getter("genrelist")
  private genrelist!: [];

    //fetchGameById
  async created() {    
        //await this.fetchData();        
        await this.fetchGameById(this.$route.params.id);
  }

  rungame() {

      var findgameloader = document.getElementById("gameloader");
      if (findgameloader !== null) { // Prevents empty list item.
        return;
      } 


      var scripts = document.getElementsByTagName('script');
      for (var i = scripts.length; i--;) {
          if (scripts[i].src.includes("data/webrtc-adapter.js")) 
            {
                scripts[i].remove();
                break;
            }
      }

      var findgamediv = document.getElementById("gamediv");
      var url = this.gameslist[0].links[this.gameslist[0].links.length-1].url;
      var platform = this.platformlist[this.gameslist[0].platform].alias;

      for (var j = this.platformlist.length; j--;) {
          if (this.platformlist[j].id == this.gameslist[0].platform) {
           platform = this.platformlist[j].alias;
           break;
          }
        }

        var alias = String(platform).replace("gbx", "gb").replace("gen", "segaMD").replace("sms","segaMS");

    console.log("find "+alias);
      var newgamediv = document.createElement("div"); // Create li element.
      newgamediv.setAttribute("id", "gameloader");
      newgamediv.setAttribute("style", "width:640px;height:480px;max-width:100%;");
      var b = document.createElement('div');
      b.id = 'game';
      newgamediv.appendChild(b);
      
      var scriptnes = document.createElement('script');
      scriptnes.innerHTML = "EJS_player = '#game'; EJS_gameName = '" + this.gameslist[0].name + "'; EJS_biosUrl = ''; EJS_gameUrl = '" + url + "'; EJS_core = '"+alias+"'; EJS_pathtodata = '/data/'; ";
      findgamediv?.appendChild(scriptnes);
      
      var scriptnes2 = document.createElement('script');
      scriptnes2.src = '/data/loader.js';
      findgamediv?.appendChild(scriptnes2);
      
      
      findgamediv?.appendChild(newgamediv);
      
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