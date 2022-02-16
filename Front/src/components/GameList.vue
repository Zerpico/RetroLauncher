<template>   
    <sui-list divided relaxed>
      <sui-list-item v-for="game in gameslist" :key="game.id">
        <sui-list-content>
          <router-link :to="'/game/'+game.id" is="sui-list-header">
          <div class="layout-container layout-container-all">
            <div class="layout-col ">
              <sui-image :src="game.links[0].url"  size="medium" rounded style="max-width: 105px" />
              <sui-list-item style="color: rgb(63, 63, 63);text-align: -webkit-center;"><sui-rating :rating="game.rating" :max-rating="5" /></sui-list-item>
            </div>
            <div class="layout-col layout-container-all" style="margin-left: 1rem; width: 100%;">
              <div style="height:100%">
                  <sui-list-item style="color: #3f3f3f"><h3>{{game.name}}</h3></sui-list-item>    
                  <sui-list-item v-if="game.alternative !== game.name" style="color: #7a7a7a">{{ game.alternative }}</sui-list-item>   
              </div>

              <div style="margin-top:1rem">
                  <sui-list-item style="color: rgb(107 137 114);">
                    <div>
                    {{ getPlatformName(game.platform) }} 
                    <img :src="getPlatformIcon(game.platform)" style="width:16px; vertical-align: text-bottom; margin-left:.5rem"/>
                    </div>
                    </sui-list-item>
                  <sui-list-item style="color: rgb(151 140 110);">
                          <div class="listgenre">
                            <span v-for="genre of game.genres" :key="genre.id">                              
                              {{genrelist[genre].name}}
                            </span>
                          </div>                       
                        </sui-list-item>					         
                   
              </div>
            </div>
          </div>
        
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

  getPlatformName(platform: number) { 
    return this.platformlist[platform].name; 
  }

  getPlatformIcon(platform: number) { 
    switch (platform)
    {
      case 1: return "icons/nintendo_nes.png"
      case 2: return "icons/nintendo_supernes.png"
      case 4: return "icons/nintendo_game_boy_advance_sp.png"
      case 6: return "icons/sega_genesis.png"
      case 14: return "icons/sega_master_system.png"
      case 15: return "icons/nec_turbografx_16.png"
      case 16: return "icons/nintendo_game_boy_pocket.png"      
      default: return "icons/icon.png"
    }   
  }
}
</script>

