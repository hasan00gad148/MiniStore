/**
 * @NApiVersion 2.1
 * @NScriptType Restlet
 */
define(['N/record', 'N/search', 'N/error'], (record, search, error) => {

  const post = (body) => {
    const required = ['entityId', 'subsidiary', 'projectExpenseType'];
    for (const field of required) {
      if (!body[field]) throw error.create({ name: 'MISSING_FIELD', message: `Missing required field: ${field}` });
    }

    const rec = record.create({ type: record.Type.JOB, isDynamic: true });

    rec.setValue({ fieldId: 'entityid',           value: body.entityId });
    rec.setValue({ fieldId: 'subsidiary',          value: body.subsidiary });
    rec.setValue({ fieldId: 'projectexpensetype',  value: body.projectExpenseType });

    if (body.projectManager)    rec.setValue({ fieldId: 'projectmanager',   value: body.projectManager });
    if (body.jobType)           rec.setValue({ fieldId: 'jobtype',           value: body.jobType });
    if (body.currency)          rec.setValue({ fieldId: 'currency',          value: body.currency });
    if (body.description)       rec.setValue({ fieldId: 'description',       value: body.description });
    if (body.startDate)         rec.setValue({ fieldId: 'startdate',         value: new Date(body.startDate) });
    if (body.endDate)           rec.setValue({ fieldId: 'enddate',           value: new Date(body.endDate) });

    if (body.customFields) {
      for (const [fieldId, value] of Object.entries(body.customFields)) {
        rec.setValue({ fieldId, value });
      }
    }

    const id = rec.save({ ignoreMandatoryFields: false });
    return { success: true, id: String(id) };
  };

  const get = (params) => {
    const columns = [
      search.createColumn({ name: 'entityid' }),
      search.createColumn({ name: 'projectmanager' }),
      search.createColumn({ name: 'jobtype' }),
      search.createColumn({ name: 'subsidiary' }),
      search.createColumn({ name: 'currency' }),
      search.createColumn({ name: 'startdate' }),
      search.createColumn({ name: 'enddate' }),
      search.createColumn({ name: 'status' }),
    ];

    const filters = [];

    if (params.id) {
      return JSON.stringify(record.load({ type: record.Type.JOB, id: params.id }));
    }

    if (params.name) {
      filters.push(search.createFilter({ name: 'entityid', operator: search.Operator.CONTAINS, values: params.name }));
    }

    if (params.subsidiary) {
      filters.push(search.createFilter({ name: 'subsidiary', operator: search.Operator.ANYOF, values: params.subsidiary }));
    }

    const projectSearch = search.create({ type: search.Type.JOB, filters, columns });

    const results = [];
    projectSearch.run().each((result) => {
      results.push({
        id:             result.id,
        entityId:       result.getValue('entityid'),
        projectManager: result.getValue('projectmanager'),
        jobType:        result.getValue('jobtype'),
        subsidiary:     result.getValue('subsidiary'),
        currency:       result.getValue('currency'),
        startDate:      result.getValue('startdate'),
        endDate:        result.getValue('enddate'),
        status:         result.getValue('status'),
      });
      return true;
    });

    return { total: results.length, results };
  };

  return { post, get };
});
