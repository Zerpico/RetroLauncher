<template>
  <div class="ui container">   
    <div>
       <sui-button compact primary content="Prev" @click="prevPage" :class="pagenumber === 1 ? 'disabled' : ''" />
       <i>&nbsp;&nbsp;Page {{pagenumber}} of {{maxPage}}&nbsp;&nbsp;&nbsp;</i>
       <sui-button compact primary content="Next" @click="nextPage" :class="pagenumber === maxPage ? 'disabled' : ''"/>
    </div>

    <GamesLists :gamelist="allGameList" />


    <div>
       <sui-button compact primary content="Prev" @click="prevPage" :class="pagenumber === 1 ? 'disabled' : ''" />
       <i>&nbsp;&nbsp;Page {{pagenumber}} of {{maxPage}}&nbsp;&nbsp;&nbsp;</i>
       <sui-button compact primary content="Next" @click="nextPage" :class="pagenumber === maxPage ? 'disabled' : ''"/>
    </div>
   
  </div>

</template>

<script>
import { mapGetters, mapActions } from "vuex";
import GamesLists from "../components/games/GamesLists.vue";
export default {
  name: "Games",
  components: {
    GamesLists,
  },
  data() {
    return {
      pagenumber: 1,
      loading: false
    }        
  },
  methods: {
    ...mapActions(["fetchGames", "fetchGamesByPage"]),
    gopage() {
      this.fetchGamesByPage(this.pagenumber);
    },
    prevPage () {
      this.loading = true
      this.pagenumber--
      this.gopage()
    },
    nextPage () {
      this.loading = true
      this.pagenumber++
      this.gopage()
    },
  },
  computed: {
    ...mapGetters(["allGameList", "currentPage", "maxPage"]),   
  },
  async mounted() {
    this.fetchGames();    
  },
};
</script>

<style scoped>
.ui.divided.list > .item:hover {
  background: var(--custom-product-tile-bg, var(--theme-bg-secondary, #f7f7f7));
}

.ui.divided.list > .item {
  background: var(--custom-product-tile-bg, var(--theme-bg-secondary, #ededed));
}
.ui.relaxed.list:not(.horizontal) > .item:not(:last-child) {
  padding-bottom: 0.05em;
}
.ui.relaxed.list:not(.horizontal) > .item:not(:first-child) {
  padding-top: 0.05em;
}
</style>

