<template>
	<div id="app">
		<v-app>
			<p> TEST </p>
			<router-view/>
			<router-link to="Main"> MainPage </router-link> 
		</v-app>
	</div>
</template>

<script>

	export default {
		name: 'App',
		data: () => ({

		})
	};
</script>
