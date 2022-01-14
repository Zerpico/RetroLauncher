<template>
  <div class="about">
    <h1>This is an about page</h1>
    {{ forecasts }}
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
        this.forecasts = response.data
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