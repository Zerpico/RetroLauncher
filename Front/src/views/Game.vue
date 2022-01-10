<template>
  <div class="ui container">
    <GameCard :game="GameList[0]"/>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import GameCard from "../components/games/GameCard.vue";
export default {
  name: "Games",
  components: {
    GameCard,
  },
  data() {
      return {
          gameid: 0
      }
  },
  created() {
      this.gameid = this.$route.params.id
  },
  methods: {
    ...mapActions(["fetchGameById"]),    
  },
  computed: {
    ...mapGetters(["GameList"]),   
  },
  async mounted() {
    this.fetchGameById(this.gameid);    
  },
};
</script>
