<template>
    <div class="home">       
        <h4>Добро пожаловать!</h4>       
        <p>Это предварительная версия <b>Ретро Лаунчера</b></p>
        <p>Здесь вы сможете просмотреть информацию по играм на приставки: NES, SNES, Sega MasterSystem, Sega MegaDrive, TurboGrafx-16, GameBoy/Color, GameBoy Advance </p>
        <p>Воспользуйтесь <router-link to="/games">Каталогом</router-link> для просмотра</p>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";

    @Component
    export default class Home extends Vue { }
</script>