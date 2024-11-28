<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { RedisKey } from '../../domain';
import redis from '../../../api/redis';
export default defineComponent({
    name: 'KeyFolder',
    props: {
        items: {
            type: Object as PropType<Array<RedisKey>>,
            required: true
        }
    },
    setup(props) {

        return {

        }
    }
})
// function loadKeys(keyspace: string) {
//     const result = redis.getKeysSpaces(keyspace)

// }

</script>

<template>
    <div class="m-0 @apply: hover:bg-slate-300 hover:cursor-pointer" v-for="item in items" :key="item.id">
        <span class="folder" v-if="item.type === 'keyspace'">
            <i :class="[item.expanded ? 'pi pi-folder' : 'pi pi-folder-open']" />
            {{ item.name }} {{ item.type === 'keyspace' ? `(${item.count})` : '' }}
        </span>
        <span v-else>
            {{ item.name }}
        </span>
        <KeyFolder class="mr-3" v-if="item.children!.length > 0" :items="item.children!" />
    </div>
</template>

<style scoped>
.folder {
    
}
</style>