import { createApp, ref, Ref, h, App, watch, provide } from 'vue';
import { ElDialog, ElButton } from 'element-plus';

/** 动作 */
export type DialogActions = {
  visible: Ref<boolean>,
  confirmLoading: Ref<boolean>,
  turnOffLoading: () => void,
  show: () => void,
  hide: () => void,
};

/** 配置 */
export type DialogOptions = {
  title?: string,
  width?: string,
  content?: any,
  callback?: any,
};

/** 钩子 */
export type Event = () => void;
export type AfterConfirmEvent = (hide: Event, turnOffLoading: Event, callback?: Event) => void;
const afterConfirmEvent: Ref<AfterConfirmEvent> = ref(() => {});

/** 动作实现 */
let dialogInstance: App<any> | null = null;
const visible = ref<boolean>(false);
const confirmLoading = ref<boolean>(false);
const turnOffLoading = () => {
  confirmLoading.value = false;
};
const show = () => {
  visible.value = true;
};
const hide = () => {
  turnOffLoading();
  visible.value = false;
};
const handleConfirm = (callback?: any) => {
  confirmLoading.value = true;
  afterConfirmEvent.value(hide, turnOffLoading, callback);
};
const actions: DialogActions = {
  visible,
  confirmLoading,
  turnOffLoading,
  show,
  hide,
};

const createDialog = (actions: DialogActions, options: DialogOptions) => {
  dialogInstance = createApp({
    setup() {
      watch(() => actions.visible.value, (val) => {
        if (!val && dialogInstance) {
          dialogInstance.unmount();
          dialogInstance = null;
          document.body.removeChild(document.getElementById('global-dialog') as HTMLElement);
        }
      });
      provide('afterConfirm', (event: AfterConfirmEvent) => {
        afterConfirmEvent.value = event;
      });
      const Comp = () => h(
        options.content || 'div', { ...options },
      );
      const slots = {
        footer: () => (
          <div>
            <ElButton size="mini" onClick={ hide }>取消</ElButton>
            <ElButton
              size="mini" type="primary"
              onClick={ () => handleConfirm(options.callback) }
              loading={ confirmLoading.value }>确认</ElButton>
          </div>
        ),
      };
      return () => (
        <ElDialog
          width={options.width || '500px'} title={options.title}
          v-model={actions.visible.value} v-slots={slots}>
          <Comp />
        </ElDialog>
      );
    },
  });
  const elem = document.createElement('div');
  elem.id = 'global-dialog';
  document.body.append(elem);
  dialogInstance.mount('#global-dialog');
};

export const useDialog = (options: DialogOptions): DialogActions => {
  if (dialogInstance) {
    show();
  } else {
    createDialog(actions, options);
  }
  return actions;
};
