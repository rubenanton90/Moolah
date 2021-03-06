﻿using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public class RefundTransactionRequestBuilder : DataCashRequestBuilderBase, IDataCashRefundTransactionRequestBuilder
    {
        public RefundTransactionRequestBuilder(DataCashConfiguration configuration)
            : base(configuration)
        {
        }

        public XDocument Build(string originalTransactionReference, decimal amount, string captureMethod = null)
        {
            return GetDocument(
                AddCaptureMethod(TxnDetailsElement(null, amount, null, null), captureMethod),
                HistoricTxnElement(originalTransactionReference));
        }

        XElement AddCaptureMethod(XElement txnDetails, string captureMethod)
        {
            if (!string.IsNullOrWhiteSpace(captureMethod))
            {
                var cm = new XElement("capturemethod", captureMethod);
                txnDetails.Add(cm);
            }
            return txnDetails;
        }

        protected override XElement TxnDetailsElement(string merchantReference, decimal amount, string currencyCode, MCC6012 mcc6012)
        {
            return new XElement("TxnDetails", new XElement("amount", amount.ToString("0.00")));
        }

        private XElement HistoricTxnElement(string originalTransactionReference)
        {
            return new XElement("HistoricTxn",
                                new XElement("reference", originalTransactionReference),
                                new XElement("method", "txn_refund"));
        }
    }
}