<template>
  <div id="app">
    <div class="flex-container">
      <header>
        <Header />
      </header>

      <section class="content">
        <div class="ui container">
          <router-view />
        </div>
      </section>

      <footer>
        <Footer />
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Header from "./components/static/Header.vue";
import Footer from "./components/static/Footer.vue";
import { Action } from "vuex-class";
import Component from "vue-class-component";

@Component({
  components: {
    Header,
    Footer,
  }
}) 
export default class App extends Vue {   

  @Action("fetchGenres")
  private fetchGenres: any;
  
  @Action("fetchPlatforms")
  private fetchPlatforms: any;

  created () {
    this.fetchGenres();
    this.fetchPlatforms();
    // получение данных после создания компонента
   
  }
};
</script>
