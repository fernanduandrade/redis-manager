<script lang="ts">
import { defineComponent, PropType, reactive, ref } from 'vue'
import { RedisKey } from '../../domain'
import redis from '../../../api/redis'
import { useApplication } from '../../../common/store/index'
import ContextMenu from 'primevue/contextmenu'

export default defineComponent({
    name: 'KeyFolder',
    props: {
        items: {
            type: Object as PropType<Array<RedisKey>>,
            required: true
        },
        connectionId: {
            type: String,
            required: true
        }
    },
    setup(props) {
        let appStorage = useApplication()
        const localItems = reactive([...props.items])
        const menus = ref<{ [key: string]: any }>({})
        async function loadKey(key: RedisKey) {
            key.expanded = !key.expanded
            if (key.children!.length > 0)
                return
            if (key.type === 'keySpace') {
                const pattern = key.parent ? `${key.parent}:${key.name}` : key.name
                const keysSpacesResponse = await redis.getKeysSpaces(props.connectionId, pattern) as RedisKey[]
                const keyspacesResult = keysSpacesResponse.map(x => ({ ...x, children: [] }))
                key.children = keyspacesResult
            } else {
                const pattern = key.parent ? `${key.parent}:${key.name}` : key.name
                const keysSpacesResponse = await redis.getKeyValue(props.connectionId, pattern)
                appStorage?.setCurrentCacheValue(keysSpacesResponse as string)
                appStorage?.setCurrentKey(pattern)
            }
        }

        const onKeyRightClick = (event: any, id: string) => {
            event.preventDefault()
            const menu = menus.value[id];
            if (menu) {
                menu.show(event);
            }
        }

        const items = ref([
            { label: 'Copy', icon: 'pi pi-copy' },
            { label: 'Rename', icon: 'pi pi-file-edit' }
        ])
        const attachMenuRef = (el: Element, itemId: string) => {
            if (el) {
                menus.value[itemId] = el;
            }
        };

        return {
            loadKey, localItems, props, menus, items, onKeyRightClick, attachMenuRef
        }
    }
})



</script>

<template>
    <div class="ml-8 w-full " v-for="item in localItems" :key="item.id" >
        <span class="key" v-if="item.type === 'keySpace'" @click="loadKey(item)">
            <i :class="[item.expanded ? 'pi pi-folder-open' : 'pi pi-folder']" />
            {{ item.name }} {{ item.type === 'keySpace' ? `(${item.count})` : '' }}
        </span>
        <span v-else class="key" @click="loadKey(item)"  aria-haspopup="true" @contextmenu="onKeyRightClick($event, item.id)">
            <i class="pi pi-key" />
            {{ item.name }}

        </span>

        <KeyFolder v-if="item.expanded && item.children!.length > 0" :connection-id="connectionId"
            :items="item.children!" />
            <ContextMenu
                :ref="(el) => attachMenuRef(el as Element, item.id)"
                :model="items"
            />
    </div>
</template>

<style scoped>
.key {
    font-size: 20px;
    @apply hover:bg-slate-300 hover:cursor-pointer w-fit p-1 rounded-md block
}
</style>