<template>
  <div>
    <sui-loader v-if="loading" active />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'

interface Data{
        loading: boolean ;
    }

@Component
export default class AboutDetail extends Vue {

  private data: Data = {
            loading: true,
        }; 

    async mounted() {
      await new Promise(f => setTimeout(f, 1000));
      this.data.loading = false;
  }

  
}
</script>