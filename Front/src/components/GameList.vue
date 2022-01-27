<template>   
    <sui-list divided relaxed>
      <sui-list-item v-for="game in gameslist" :key="game.id">
        <sui-list-content>
          <router-link :to="'/game/'+game.id" is="sui-list-header">
            <sui-grid>
              <sui-grid-row>
                <sui-grid-column :width="2">
                  <img :src="game.links[0].url" style="max-height: 144px" />
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
                          hello
                        </span>
                       </div>                       
                    </sui-list-item>					  
                    <sui-list-item style="color: rgb(41 174 74);">{{ platformlist[game.platform].name }}</sui-list-item>
                    <sui-list-item style="color: rgb(156 147 114)">{{ game.year }}</sui-list-item>
                    <sui-list-item style="color: #3f3f3f">{{ game.publisher }}</sui-list-item>
                    <sui-list-item style="color: #3f3f3f"><sui-rating :rating="game.rating" :max-rating="5" /></sui-list-item>
                  </sui-list>
                </sui-grid-column>
              </sui-grid-row>
            </sui-grid>
          </router-link>
        </sui-list-content>
      </sui-list-item>
    </sui-list>    
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game } from "../store/game/types";
import { Genre } from "../store/genre/types";
import { Platform } from "../store/platform/types";
@Component
export default class GameListTable extends Vue {
  @Prop() gameslist:Game[]
  @Prop() platformlist:Platform[]
  @Prop() genrelist:Genre[]
}
</script>
