<script lang="ts">
import { defineComponent, PropType, ref } from 'vue';
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
        const aaa = ref(false)
        function loadKey(key: RedisKey) {
            if(key.type === 'keyspace') {
                console.log('caiu aqui')
                key.expanded = !key.expanded
                aaa.value = key.expanded && key.children!.length > 0
                console.log('tá verdadeiro essa merda?', aaa.value)
            }
        }
        return {
            loadKey, aaa
        }
    }
})


const aa = ref(false)

// function loadKeys(keyspace: string) {
//     const result = redis.getKeysSpaces(keyspace)

// }

</script>

<template>
    <div class="m-0 w-full" v-for="item in items" :key="item.id">
        <span class="key" v-if="item.type === 'keyspace'" @click="loadKey(item)">
            <i :class="[item.expanded ?  'pi pi-folder-open' : 'pi pi-folder']" />
            {{ item.name }} {{ item.type === 'keyspace' ? `(${item.count})` : '' }}
        </span>
        <span v-else class="key ml-4" @click="loadKey(item)">
            {{ item.name }}
        </span>
        <KeyFolder class="ml-4" v-if="aaa" :items="item.children!" />
    </div>
</template>

<style scoped>

.key {
    @apply hover:bg-slate-300 hover:cursor-pointer
}
</style>