import { createApp, ref, Ref, h, App, watch } from 'vue';
import { ElDialog, ElButton } from 'element-plus';

export type DialogActions = {
  visible: Ref<boolean>,
  confirmLoading: Ref<boolean>,
  turnOffLoading: () => void,
  show: () => void,
  hide: () => void,
};

export type DialogOptions = {
  title?: string,
  width?: string,
  content?: any,
  callback?: any,
};

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
const handleConfirm = () => {
  confirmLoading.value = true;
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
        }
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
              onClick={ handleConfirm }
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
