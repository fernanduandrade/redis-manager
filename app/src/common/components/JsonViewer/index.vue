<script lang="ts" setup>

defineProps({
    content: {
        type: String,
        required: true
    }
})

function syntaxHighlight(json: string) {
    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, (match) => {
        let style = '';
        if (/^"/.test(match)) {
            if (/:$/.test(match)) style = 'color: red;';
            else style = 'color: green;';
        } else if (/true|false/.test(match)) {
            style = 'color: blue;';
        } else if (/null/.test(match)) {
            style = 'color: magenta;';
        } else if (/^-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?$/.test(match)) {
            style = 'color: darkorange;';
        }
        return `<span style="${style}">${match}</span>`;
    });
}

</script>


<template>
    <pre v-html="syntaxHighlight(content)"></pre>
</template>

<style>
pre {
    outline: 1px solid #ccc;
    padding: 5px;
    margin: 5px;
}
</style>