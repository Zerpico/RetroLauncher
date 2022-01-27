<template>
    <div>
        <sui-grid divided="vertically">
            <sui-grid-row :columns="2">
            <sui-grid-column :width="9">
                <h3 style="margin-bottom: 0px;">{{ gameslist[0].name }}</h3>
                <h4 style="margin-top: 0px;color: #7a7a7a" v-if="gameslist[0].alternative !== gameslist[0].name">{{ gameslist[0].alternative }}</h4> 
            </sui-grid-column>
            <sui-grid-column :width="7">
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

        <sui-grid divided="vertically">
            <sui-grid-row :columns="2">
            <sui-grid-column :width="9" style="margin-top: 0.8rem">
                {{ gameslist[0].annotation }}
            </sui-grid-column>
            <sui-grid-column :width="7">
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
            </sui-grid-column>
            </sui-grid-row>
        </sui-grid>

        <div id="gamediv" style="margin-top:1.5rem"></div>
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

      const findgameloader = document.querySelector("#gamediv");
      if (findgameloader?.childNodes.length > 0) { // Prevents empty list item.
        findgameloader?.scrollIntoView();
        return;
      } 
  
      var embed = document.createElement("div");
      embed.className="embed";

      var iframe = document.createElement("iframe");
      iframe.setAttribute("src","https://retro.khudaev.ru/api/Games/GetEmulById?Id="+this.gameslist[0].id);
      iframe.setAttribute("frameborder","0");
      iframe.setAttribute("width","100%");
      iframe.setAttribute("height","100%");
      iframe.setAttribute("scrolling","no");

      embed.appendChild(iframe);
      embed.style.height="480px";
 
      findgameloader?.appendChild(embed);
      findgameloader?.scrollIntoView();
      
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