using StyleCop.CSharp;

namespace StyleCop.SpecialRules
{
    [SourceAnalyzer(typeof(CsParser))]
    public class SpecialAnalyzer : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            var codeDocument = (CsDocument)document;

            if (codeDocument.RootElement != null && !codeDocument.RootElement.Generated)
            {
                var statementCallback = new CodeWalkerStatementVisitor<object>(VisitStatement);

                codeDocument.WalkDocument(null, statementCallback, null);
            }
        }

        private bool VisitStatement(Statement statement, 
                                    Expression parentExpression, 
                                    Statement parentStatement, 
                                    CsElement parentElement, 
                                    object context)
        {
            Param.AssertNotNull(statement, "statement");
            Param.Ignore(parentExpression);
            Param.Ignore(parentStatement);
            Param.AssertNotNull(parentElement, "parentElement");
            Param.Ignore(context);

            switch (statement.StatementType)
            {
                case StatementType.If:
                    {
                        CheckSingleIfStatement(parentElement, (IfStatement)statement);
                        break;
                    }

                case StatementType.Else:
                    {
                        CheckMissingBlock(parentElement,
                                          statement,
                                          ((ElseStatement)statement).EmbeddedStatement,
                                          statement.FriendlyTypeText,
                                          false);

                        break;
                    }

                case StatementType.While:
                    {
                        CheckMissingBlock(parentElement,
                                          statement,
                                          ((WhileStatement)statement).EmbeddedStatement,
                                          statement.FriendlyTypeText,
                                          false);

                        break;
                    }

                case StatementType.For:
                    {
                        CheckMissingBlock(parentElement,
                                          statement,
                                          ((ForStatement)statement).EmbeddedStatement,
                                          statement.FriendlyTypeText,
                                          false);

                        break;
                    }

                case StatementType.Foreach:
                    {
                        CheckMissingBlock(parentElement,
                                          statement,
                                          ((ForeachStatement)statement).EmbeddedStatement,
                                          statement.FriendlyTypeText,
                                          false);

                        break;
                    }

                case StatementType.DoWhile:
                    {
                        CheckMissingBlock(parentElement,
                                          statement,
                                          ((DoWhileStatement)statement).EmbeddedStatement,
                                          statement.FriendlyTypeText,
                                          false);

                        break;
                    }

                case StatementType.Using:
                    {
                        CheckMissingBlock(parentElement,
                                          statement,
                                          ((UsingStatement)statement).EmbeddedStatement,
                                          statement.FriendlyTypeText,
                                          true);

                        break;
                    }

                case StatementType.Lock:
                    {
                        CheckMissingBlock(parentElement,
                                          statement,
                                          ((LockStatement)statement).EmbeddedStatement,
                                          statement.FriendlyTypeText,
                                          false);

                        break;
                    }

                default:
                    break;
            }

            return true;
        }

        private void CheckSingleIfStatement(CsElement parentElement, IfStatement ifStatement)
        {
            var embeddedStatement = ifStatement.EmbeddedStatement;

            if (!IsAllowedEmbeddedStatementType(embeddedStatement.StatementType) ||
                ifStatement.AttachedElseStatement != null ||
                ifStatement.LineNumber != embeddedStatement.LineNumber)
            {
                CheckMissingBlock(parentElement,
                                  ifStatement,
                                  embeddedStatement,
                                  ifStatement.FriendlyTypeText,
                                  false);
            }
        }

        private bool IsAllowedEmbeddedStatementType(StatementType type)
        {
            return type == StatementType.Expression ||
                   type == StatementType.Return ||
                   type == StatementType.Break ||
                   type == StatementType.Throw;
        }

        private void CheckMissingBlock(CsElement parentElement, 
                                       Statement statement, 
                                       Statement embeddedStatement, 
                                       string statementType, 
                                       bool allowStacks)
        {
            Param.AssertNotNull(parentElement, "parentElement");
            Param.AssertNotNull(statement, "statement");
            Param.Ignore(embeddedStatement);
            Param.AssertValidString(statementType, "statementType");
            Param.Ignore(allowStacks);

            if (embeddedStatement != null && 
                embeddedStatement.StatementType != StatementType.Block)
            {
                if (!allowStacks || 
                    statement.ChildStatements == null || 
                    statement.ChildStatements.Count == 0)
                {
                    AddViolation(parentElement, 
                                 embeddedStatement.LineNumber, 
                                 SpecialRules.CurlyBracketsMustNotBeOmitted, 
                                 statementType);
                }
            }
        }
    }
}
