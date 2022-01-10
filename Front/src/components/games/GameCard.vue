<template>
  <div class="ui container">
    <h3>{{ game.name }}</h3>
    <h4 v-if="game.alternative !== game.name">{{ game.alternative }}</h4>

    <div>      
      <sui-image-group size="small" v-for="link in game.links" :key="link.id">
        <a v-if="link.type !== 'rom'">
          <sui-image :src="link.url" />
        </a>
      </sui-image-group>
    </div>
  </div>
</template>

<script>
export default {
  name: 'GameCard',
  props: {
    game: null
  }
}
</script>