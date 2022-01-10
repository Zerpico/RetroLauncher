<template>
  <sui-grid celled>
    <sui-grid-row v-for="game in gamelist" :key="game.id">
      <sui-grid-column :width="2">
        <img :src="game.links[0].url" style="width:8em" />         
      </sui-grid-column>
      <sui-grid-column :width="14">
        <h4><router-link :to="{ name: 'game', params: {id: game.id} }">{{ game.name }}</router-link></h4>
        <p>{{ game.alternative }}</p>
        <p>{{ game.year }}</p>     
        <p>{{ game.publisher }}</p>       
      </sui-grid-column>
    </sui-grid-row>      
  </sui-grid>
</template>

<script>
export default {
  name: 'GamesLists',
  props: {
    gamelist: Array
  }
}
</script>

<style>
.ui.divided.list > .item:hover {
  border-top: 1px solid rgba(34, 36, 38, 0.15);
  background: var(--custom-product-tile-bg, var(--theme-bg-secondary, #f7f7f7));
}

.ui.divided.list > .item {
  background: var(--custom-product-tile-bg, var(--theme-bg-secondary, #ededed));
}
</style>
