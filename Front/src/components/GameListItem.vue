<template>
    <sui-grid-row>
      <sui-grid-column :width="3">
        {{ post.genre }}
      </sui-grid-column>
      <sui-grid-column :width="13">
        {{ post.title }}
      </sui-grid-column>
    </sui-grid-row>
    
</template>

<script>
export default {
  name: 'GameListItem',
  props: ['post'],
}  
</script>