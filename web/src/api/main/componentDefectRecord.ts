import request from '/@/utils/request';
enum Api {
	AddComponentDefectRecord = '/api/componentDefectRecord/add',
	DeleteComponentDefectRecord = '/api/componentDefectRecord/delete',
	UpdateComponentDefectRecord = '/api/componentDefectRecord/update',
	PageComponentDefectRecord = '/api/componentDefectRecord/page',
	GetUploadTemplateComponentDefectRecord = '/api/componentDefectRecord/GetUploadTemplate',
	ExportComponentDefectRecord = "/api/componentDefectRecord/Export"
}

// 增加部件缺陷记录
export const addComponentDefectRecord = (params?: any) =>
	request({
		url: Api.AddComponentDefectRecord,
		method: 'post',
		data: params,
	});

// 删除部件缺陷记录
export const deleteComponentDefectRecord = (params?: any) =>
	request({
		url: Api.DeleteComponentDefectRecord,
		method: 'post',
		data: params,
	});

// 编辑部件缺陷记录
export const updateComponentDefectRecord = (params?: any) =>
	request({
		url: Api.UpdateComponentDefectRecord,
		method: 'post',
		data: params,
	});

// 分页查询部件缺陷记录
export const pageComponentDefectRecord = (params?: any) =>
	request({
		url: Api.PageComponentDefectRecord,
		method: 'post',
		data: params,
	});

export const getUploadTemplateComponentDefectRecord = () => request({
	url: Api.GetUploadTemplateComponentDefectRecord,
	method: 'get',
	responseType: 'blob'
});

export const exportComponentDefectRecord = (params?: any) => request({
	url: Api.ExportComponentDefectRecord,
	method: 'get',
	responseType: 'blob',
	data: params
});
