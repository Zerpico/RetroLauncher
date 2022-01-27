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
	    		    <div class="details__content table__row-content">{{ platformlist[gameslist[0].platform].name }}</div>
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

.title-underline-text{
    justify-content: space-between;
    font-size: 16px;
    line-height: 19px;
    font-weight: 600;
    border-bottom: 1px solid #bfbfbf;
    margin-bottom: 20px;
}

.layout-container {
  box-sizing: content-box;
  position: relative;
  max-width: 1096px;
  margin: auto;
  /*! padding: 0 16px; */
  display: inline-flex;
}

.layout-side-col {
  width: 38.68613%;
}
.layout-main-col, .layout-side-col {
  display: inline-block;
  vertical-align: top;
}
.layout-main-col {
  width: 56.93431%;
  margin-right: 4.37956%;
}


.details {
  margin-bottom: 50px;
  padding-top: 10px;
}
.table {
  width: 100%;
}

.details .details__row {
  padding-bottom: 8px;
}
.details__row:hover:before {
 content:"";
 position:absolute;
 top:0;
 bottom:0;
 left:-8px;
 right:-8px;
 background:hsla(0,0%,100%,.2);
 border-left:2px solid #78387b
}

.table--without-border .table__row {
  border-bottom: none;
  /*! padding-bottom: 4px; */
}
.details__row {
  position: relative;
  padding-top: 7px;
}
.table__row {
  display: -ms-flexbox;
  display: flex;
  padding: 12px 0;
    padding-top: 12px;
    padding-bottom: 12px;
  line-height: 17px;
  font-size: 14px;
  border-bottom: 1px solid #bcbcbc;
}

.details__category, .details__content {
  line-height: 17px;
  z-index: 10;
}
.details__category {
  width: 110px;
}
.table-header, .table__row-label {
  color: rgba(33,33,33,.7);
}
.table__row-content, .table__row-label {
  vertical-align: top;
}

.title--no-margin {
  margin-bottom: 0;
}
.title {
  display: -ms-flexbox;
  display: flex;
  -ms-flex-pack: justify;
  justify-content: space-between;
  font-size: 16px;
  line-height: 19px;
  font-weight: 600;
  border-bottom: 1px solid #bfbfbf;
  margin-bottom: 20px;
}
.title__underline-text {
  position: relative;
  padding: 16px 0;
  line-height: 12px;
  border-bottom: 1px solid;
  margin-bottom: -1px;
}
.description {
    margin-top: 0.5em;
    margin-bottom: 50px;
    font-size: 14px;
    font-weight: 400;
    line-height: 21px;
	padding: 22px 0 10px;
}
</style>