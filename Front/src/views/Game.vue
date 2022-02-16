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

      

        <div class="img-scroller">
          <ul>
            <li v-for="link of filterlinks(gameslist[0].links)" :key="link.id">
              <a @click.prevent="toggle(link)" href="#">
                <img class="img-scroller-item" :src="link.url"  style="max-height: 144px; width:auto" />
              </a>
            </li>           
          </ul>
        </div>

        <sui-modal v-model="data.openimg" class="img-modal" @click.prevent="toggle(null)"  >
          <sui-modal-content image>
            <sui-image size="huge" :src="data.currentimg.url" style="max-height: 468px; width: auto;" />           
          </sui-modal-content>       
          <sui-modal-actions>
          <sui-button positive @click.prevent="previmg">Пред.</sui-button>
          <sui-button positive @click.prevent="nextimg">След.</sui-button>
        </sui-modal-actions>  
        </sui-modal>
       

        <div class="layout-container">

	    <div class="layout-main-col">
	    	<div class="content-summary-section content-summary-offset">
	    		<div class="title ">
	    			<div class="title__underline-text">Описание</div>
	    			<div class="title__additional-options"></div>
	    		</div>
	    		<div class="description">
	    			<i>{{ gameslist[0].annotation }}</i>
	    		</div>
	    	</div>
	    </div>

	    <div class="layout-side-col">
	    	<div class="title title--no-margin">
	    		<div class="title__underline-text">Детали игры</div>
	    		<div class="title__additional-options"></div>
	    	</div>    

	    	<div class="details table table--without-border">
                <div class="table__row details__rating details__row ">
                    <div class="details__category table__row-label">Платформа:</div>
	    		    <div class="details__content table__row-content">
                {{ platformlist[gameslist[0].platform].name }}
                <img :src="getPlatformIcon(gameslist[0].platform)" style="width:16px; vertical-align: text-bottom; margin-left:.5rem"/>
                </div>
	    		</div>
	    		<div class="table__row details__row">
	    			<div class="details__category table__row-label">Жанр:</div>
	    			<div class="details__content table__row-content">
                       <span v-for="genre of gameslist[0].genres" :key="genre.id">
                                {{genrelist[genre].name}}
                        </span>   				
	    			</div>
	    		</div>
	    		<div class="table__row details__rating details__row ">
                    <div class="details__category table__row-label">Дата выхода:</div>
	    		    <div class="details__content table__row-content">{{ gameslist[0].year }}</div>
	    		</div>
                <div class="table__row details__rating details__row ">
                    <div class="details__category table__row-label">Разработчик:</div>
	    		    <div class="details__content table__row-content">{{ gameslist[0].publisher }}</div>
	    		</div>
	    	</div>
	    </div>	
        </div>

        <div id="gamediv" style="margin-top:1.5rem"></div>
    </div>
</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import { State, Action, Getter } from "vuex-class";
import { GameState, Game, Link, GameRequest } from "../store/game/types";
import { Platform } from "../store/platform/types";

interface Data{
            loading: boolean;    
            openimg: boolean;   
            currentimg: Link;
    }

@Component
export default class GameView extends Vue {
  private data: Data = {
            loading: true,
            openimg: false,
            currentimg: {} as Link
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

  filterlinks(links: Link[]){
    return links.filter((l) => l.type != 'rom');
  }

  toggle(link: Link | null) {
    if (link)
      this.data.currentimg = link;
      this.data.openimg = !this.data.openimg;
  }
 
  previmg() {
    const imgs = this.filterlinks(this.gameslist[0].links);
    const ind = imgs.findIndex(x => x === this.data.currentimg);
    if (ind === 0) {
      this.data.currentimg = imgs[imgs.length-1];
    }
    else {
      this.data.currentimg = imgs[ind-1];
    }
  }

  nextimg() {
    const imgs = this.filterlinks(this.gameslist[0].links);
    const ind = imgs.findIndex(x => x === this.data.currentimg);
    if (ind === imgs.length-1) {
      this.data.currentimg = imgs[0];
    }
    else {
      this.data.currentimg = imgs[ind+1];
    }
  }

  getPlatformIcon(platform: number) { 
    switch (platform)
    {
      case 1: return "../icons/nintendo_nes.png"
      case 2: return "../icons/nintendo_supernes.png"
      case 4: return "../icons/nintendo_game_boy_advance_sp.png"
      case 6: return "../icons/sega_genesis.png"
      case 14: return "../icons/sega_master_system.png"
      case 15: return "../icons/nec_turbografx_16.png"
      case 16: return "../icons/nintendo_game_boy_pocket.png"      
      default: return "../icons/icon.png"
    }   
  }

  rungame() {

      const findgameloader = document.querySelector("#gamediv");
      const lenght = findgameloader?.childNodes?.length ? 0 : Number(findgameloader?.childNodes?.length);
      if (lenght > 0) { // Prevents empty list item.
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



.img-modal .modal {
 width: auto;
}


.list span:not(:last-child)::after {
  content: ',';
}


.img-scroller {
  display: flex;
  justify-content: left; /* horizontal alignment / centering of the ul */  
  max-width: 100%; /* responsiveness */
  margin: 0 auto; /* horizontal alignment / centering on the page */
}

.img-scroller > ul {
  list-style: none;
  display: flex;
  overflow: auto; /* better this way */
  overflow-y: hidden; /* appears just a little, don't know why (yet), needs to be set to hide it and make it look nicer */
  max-height: 100vh;
}

.img-scroller > ul > li {
  height: 150px; /* needs to match the height of the "shortest" img or be less than that but not more */
  flex: 0 0 auto; /* mandatory */
  max-height: 100vh;
}

.img-scroller-item { /* responsiveness */
  display: block; /* to remove the bottom margin */
  height: 100vh;
  max-width: 100%;
  max-height: 100%; /* mandatory */
  margin: 0 .5rem;
}


#game{
    margin-top: 3rem;
}

</style>