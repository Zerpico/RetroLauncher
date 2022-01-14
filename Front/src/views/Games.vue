<template>
  <div class="about">    
	
	<div>
		<sui-message v-if="showError" info>
		  {{ errorMessage }}
		</sui-message>

		<sui-loader v-if="loading" active />
	
	
	  <sui-list divided relaxed>
		<sui-list-item v-for="game in forecasts" :key="game.id">		  
		  <sui-list-content>
			<a href="#" is="sui-list-header">
			
			<sui-grid>
			  <sui-grid-row>
			    <sui-grid-column :width="2">
					<img :src="game.links[0].url" style="width:8em" />
			    </sui-grid-column>
			    <sui-grid-column :width="13">
				  <sui-list>
					<sui-list-item style="color: #3f3f3f;">{{ game.name }}</sui-list-item>
					<sui-list-item v-if="game.alternative !== game.name" style="color: #7a7a7a;">{{ game.alternative }}</sui-list-item>
					<sui-list-item style="color: rgb(156 147 114);">{{ game.year }}</sui-list-item>
					<sui-list-item style="color: #3f3f3f;">{{ game.publisher }}</sui-list-item>
					<sui-list-item style="color: #3f3f3f;"><sui-rating :rating="game.rating" :max-rating="5" /></sui-list-item>
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
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from 'vue'

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading weather forecast.',
      forecasts: []
    }
  },
  methods: {    
    async fetchWeatherForecasts() {
      try {
        const response = await this.$axios.get('https://retro.khudaev.ru/api/Games/GetByName?Name=mario')
        this.forecasts = response.data.data.games
      } catch (e) {
        this.showError = true
        this.errorMessage = `Error while loading weather forecast: ${e.message}.`
      }
      this.loading = false
    }
  },
  async created() {
    await this.fetchWeatherForecasts()
  }
})  
</script>

<style>
.ui.divided.list > .item:hover {
  border-top: 1px solid rgba(34, 36, 38, 0.15);
  background: var(--custom-product-tile-bg, var(--theme-bg-secondary, #c0c0c0));
}

</style>