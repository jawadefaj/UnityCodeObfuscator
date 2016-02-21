using System;

using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using Mono.TextEditor;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Refactoring.Rename;
using MonoDevelop.Core;
using MonoDevelop.Refactoring;
using System.Collections.Generic;


namespace DateInserter
{

	class InsertDateHandler : AbstractRefactoringCommandHandler
	{
		NewRefector NewRefector = new NewRefector ();
		int index = 0;
		protected override void Update(RefactoringOptions options, CommandInfo info)
		{
			info.Enabled = true;
			/*
			LoggingService.LogDebug ("-------------------------------");
			LoggingService.LogDebug (options.SelectedItem.GetType().ToString());
			LoggingService.LogDebug ("-------------------------------");
			*/
			RenameRefactoring renameRefactoring = new RenameRefactoring();
			if (!renameRefactoring.IsValid(options))
			{
				LoggingService.LogDebug ("inside if");
			}
		}

		protected override void Run(RefactoringOptions options)
		{

			RenameRefactoring renameRefactoring = new RenameRefactoring();
			if (renameRefactoring.IsValid (options)) {
				if (options.SelectedItem is ITypeDefinition) {
					ITypeDefinition classname = options.SelectedItem as ITypeDefinition;
					//IEnumerable<IMethod> methodList = classname.Methods;
					IList<IMember> memList = classname.Members;
					foreach (IMember m in memList) {
						IEnumerable<IAttribute> attributelist = m.GetAttributes (true);
						/*
						 * 
						 * 
						if (m is IParameterizedMember) {
							IParameterizedMember iMem = m as IParameterizedMember;
							LoggingService.LogDebug ("#################################################");
							LoggingService.LogDebug ("name of de iMemName " + iMem.Name);
							IList<IParameter> para = iMem.Parameters;

							for(int i = 0 ; i<para.Count; i++){
								//ISymbol pMetre = para[i] as ISymbol;
								//LoggingService.LogDebug ("pMetre  " + pMetre.Name);
								//IParameter p;
								IVariable iVar = para[i] as IVariable;
								LoggingService.LogDebug ("name1  " + iVar.Name);
								RenameRefactoring.RenameVariable (iVar, NewRefector.MethodName [index++]);
								//LoggingService.LogDebug ("name2  " + iVar.Name);
			
							}
							LoggingService.LogDebug ("#################################################");
						}

							*/


						
						if (m is IVariable) {
							if (m.IsPrivate) {
								bool change = true;
								foreach (IAttribute attribute in attributelist) {
									if (attribute.AttributeType.Name == "SerializeField" ) {
										change = false;
										LoggingService.LogDebug ("serialized thats why mot changed " + m.Name);
									}
								}
								if (change) {
									IVariable var = m as IVariable;
									RenameRefactoring.RenameVariable (var, NewRefector.MethodName [index++]);
									LoggingService.LogDebug (" variable " + m.FullName);
								}
							} 
							else if (m.IsPublic) {
								foreach (IAttribute attribute in attributelist) {
									if (attribute.AttributeType.Name == "Change") {
										IVariable var = m as IVariable;
										RenameRefactoring.RenameVariable (var, NewRefector.MethodName [index++]);
									}
								}
							}
						}
						else {
							bool changeMethod = true;
							foreach (IAttribute attr in attributelist) {
								if (attr.AttributeType.Name == "DoNotChange") {
									changeMethod = false;
								}
							}
							if(NewRefector.hasValue(m.Name)){
								changeMethod = false;
							}
							if (m.IsAbstract || m.IsVirtual || m.IsOverride) {
								changeMethod = false;
							}

							if (changeMethod) {
								IEntity Method = m as IEntity;
								LoggingService.LogDebug ("change method "+ m.Name);
								//RenameRefactoring.Rename (Method, NewRefector.MethodName [index++]);
							}
						}
					}
				}
			}
		}
	}
}

