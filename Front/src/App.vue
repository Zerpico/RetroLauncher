
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

<script>
import Header from "./components/static/Header.vue";
import Footer from "./components/static/Footer.vue";
export default {
  name: "App",
  components: {
    Header,
    Footer,
  },
};
</script>